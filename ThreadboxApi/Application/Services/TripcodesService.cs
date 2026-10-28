using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using ThreadboxApi.Application.Common;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

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
            string[] fragments = tripcodeString.Split('#');
            string key = fragments.First();
            string value = fragments.Last();

            Tripcode tripcode = await _dbContext.Tripcodes
                .AsNoTracking()
                .Where(x => x.Key == key)
                .SingleOrDefaultAsync(cancellationToken);

            if (tripcode == null)
            {
                byte[] salt = RandomNumberGenerator.GetBytes(16);

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
                throw new HttpResponseException("Incorrect tripcode value.");
            }

            return tripcode;
        }

        private static byte[] Hash(string value, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password: value,
                salt: salt, 
                iterations: 10000,
                hashAlgorithm: HashAlgorithmName.SHA256);

            return pbkdf2.GetBytes(32);
        }
    }
}