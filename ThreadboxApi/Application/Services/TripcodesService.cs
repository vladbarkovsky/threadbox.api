using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.Exceptions;

namespace ThreadboxApi.Application.Services
{
    public class TripcodesService : IScopedService
    {
        private readonly ApplicationDbContext _dbContext;

        public TripcodesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates and returns new tripcode if there are no existing tripcode with key specified
        /// in <paramref name="tripcodeString"/>.
        /// Otherwise returns existing tripcode if its value is equal to specified
        /// in <paramref name="tripcodeString"/>.
        /// </summary>
        public async Task<Tripcode> ProcessTripcodeStringAsync(string tripcodeString, CancellationToken cancellationToken)
        {
            var fragments = tripcodeString.Split('#');
            var key = fragments.First();
            var value = fragments.Last();

            var tripcode = await _dbContext.Tripcodes
                .Where(x => x.Key == key)
                .SingleOrDefaultAsync(cancellationToken);

            if (tripcode == null)
            {
                var salt = RandomNumberGenerator.GetBytes(16);

                tripcode = new Tripcode
                {
                    Key = key,
                    Salt = salt,
                    Hash = Hash(value, salt)
                };

                return tripcode;
            }

            if (!Hash(value, tripcode.Salt).SequenceEqual(tripcode.Hash))
            {
                throw new HttpStatusException("Incorrect tripcode value.");
            }

            return tripcode;
        }

        private byte[] Hash(string value, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(value, salt, 10000, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(32);
        }
    }
}