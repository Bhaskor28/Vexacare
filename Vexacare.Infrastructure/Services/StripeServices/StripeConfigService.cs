using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vexacare.Domain.Entities.Stripe;
using Vexacare.Infrastructure.Data;

namespace Vexacare.Infrastructure.Services.StripeServices
{
    public class StripeConfigService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _protector;


        // The purpose string must be unique for this specific use case
        private const string ProtectorPurpose = "StripeSecretKeyProtection.v1";

        public StripeConfigService(ApplicationDbContext context, IDataProtectionProvider protectionProvider)
        {
            _context = context;
            // Create a protector specifically for Stripe keys
            _protector = protectionProvider.CreateProtector(ProtectorPurpose);

        }

        // Get the current config (decrypting the secret key)
        public async Task<StripeConfig?> GetConfigAsync()
        {
            var config = await _context.StripeConfigs.FirstOrDefaultAsync();

            if (config != null && !string.IsNullOrEmpty(config.SecretKey))
            {
                // Decrypt the secret key when retrieving it
                try
                {
                    config.SecretKey = config.SecretKey;
                }
                catch
                {
                    config.SecretKey = "[Decryption Failed]";
                }
            }
            return config;
        }

        // Save or update the config (encrypting the secret key)
        public async Task SaveConfigAsync(StripeConfig newConfig)
        {
            var existingConfig = await _context.StripeConfigs.FirstOrDefaultAsync();
            //string encryptedSecretKey = _protector.Protect(newConfig.SecretKey);
            string SecretKey = newConfig.SecretKey;

            if (existingConfig == null)
            {
                // Create a new record
                var configToSave = new StripeConfig
                {
                    PublishableKey = newConfig.PublishableKey,
                    SecretKey = SecretKey,
                    IsTestMode = newConfig.IsTestMode
                };
                _context.StripeConfigs.Add(configToSave);
            }
            else
            {
                // Update the existing record
                existingConfig.PublishableKey = newConfig.PublishableKey;
                existingConfig.SecretKey = SecretKey;
                existingConfig.IsTestMode = newConfig.IsTestMode;
                _context.StripeConfigs.Update(existingConfig);
            }

            await _context.SaveChangesAsync();
        }

        // A helper method to get the keys ready for use in payment processing
        public async Task<(string PublishableKey, string SecretKey)> GetStripeKeysForPaymentAsync()
        {
            var config = await GetConfigAsync();
            if (config == null)
            {
                throw new Exception("Stripe configuration not found. Please configure Stripe keys in the admin dashboard.");
            }
            return (config.PublishableKey, config.SecretKey);
        }
    }
}
