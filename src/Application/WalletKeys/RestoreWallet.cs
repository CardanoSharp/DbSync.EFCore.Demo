using CardanoSharp.Wallet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions;
using CardanoSharp.Wallet.Encoding;

namespace Application.WalletKeys
{
    public static class RestoreWallet
    {
        public record RestoreWalletDataCommand(string mnemonic) : IRequest<RestoreWalletDataResponse>;

        public class RestoreWalletDataHandler : IRequestHandler<RestoreWalletDataCommand, RestoreWalletDataResponse>
        {
            private readonly IKeyService _keyService;
            private readonly IAddressService _addressService;
            private readonly IBech32 _bech32;
            private readonly ILogger<RestoreWalletDataResponse> _logger;

            public RestoreWalletDataHandler(
                IKeyService keyService, 
                IAddressService addressService, 
                IBech32 bech32,
                ILogger<RestoreWalletDataResponse> logger)
            {
                _keyService = keyService;
                _addressService = addressService;
                _bech32 = bech32;
                _logger = logger;
            }

            public async Task<RestoreWalletDataResponse> Handle(RestoreWalletDataCommand request, CancellationToken cancellationToken)
            {
                var entropy = _keyService.Restore(request.mnemonic);
                var master = _keyService.GetRootKey(entropy);

                var rootKey = new byte[master.Item1.Length + master.Item2.Length];
                Buffer.BlockCopy(master.Item1, 0, rootKey, 0, master.Item1.Length);
                Buffer.BlockCopy(master.Item2, 0, rootKey, master.Item1.Length, master.Item2.Length);

                var paymentPath = "m/1852'/1815'/0'/0/0";
                var paymentPrv = _keyService.DerivePath(paymentPath, master.Item1, master.Item2);
                var paymentPub = _keyService.GetPublicKey(paymentPrv.Item1, false);

                var paymentPubCC = new byte[paymentPub.Length + paymentPrv.Item2.Length];
                Buffer.BlockCopy(paymentPub, 0, paymentPubCC, 0, paymentPub.Length);
                Buffer.BlockCopy(paymentPrv.Item2, 0, paymentPubCC, paymentPub.Length, paymentPrv.Item2.Length);

                var stakePath = "m/1852'/1815'/0'/2/0";
                var stakePrv = _keyService.DerivePath(stakePath, master.Item1, master.Item2);
                var stakePub = _keyService.GetPublicKey(stakePrv.Item1, false);

                var stakePubCC = new byte[stakePub.Length + stakePrv.Item2.Length];
                Buffer.BlockCopy(stakePub, 0, stakePubCC, 0, stakePub.Length);
                Buffer.BlockCopy(stakePrv.Item2, 0, stakePubCC, stakePub.Length, stakePrv.Item2.Length);

                var baseAddr = _addressService.GetAddress(paymentPub, stakePub, NetworkType.Testnet, AddressType.Base);

                return new RestoreWalletDataResponse(
                    entropy.ToStringHex(), 
                    _bech32.Encode(rootKey, "root_xsk"),
                    _bech32.Encode(paymentPubCC, "addr_xvk"),
                    _bech32.Encode(stakePubCC, "stake_xvk"),
                    baseAddr);
            }
        }

        public record RestoreWalletDataResponse(string Entropy, string RootKey, string PublicKey, string StakeKey, string Address);
    }
}
