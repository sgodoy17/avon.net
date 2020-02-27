using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Component.Transversal.Cryptography
{
    /// <summary>
    ///     Verifies output and performance of the hashing algorithms used in the solution.
    /// </summary>
    [TestFixture]
    public class PasswordHashTests
    {
        /// <summary>
        ///     Creates and validates BCrypt hashes
        /// </summary>
        [Test]
        public void CheckBCryptCreateAndVerify()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            const int totalItterations = 50;

            for (int i = 0; i < totalItterations; i++)
            {
                string password = Guid.NewGuid().ToString();
                string salt = Guid.NewGuid().ToString();

                Assert.IsTrue(
                    PasswordHasher.DatabaseAuthenticateUser(PasswordHasher.GetKey(password, salt),
                        PasswordHasher.CreateDatabaseHash(PasswordHasher.GetKey(password, salt))),
                    "BCrypt hash was not generated successfully");
            }
            stopwatch.Stop();
            Console.WriteLine("Total execution time per test: {0} ms",
                stopwatch.ElapsedMilliseconds/1.0/totalItterations);
        }

        /// <summary>
        ///     Checks the performance of the BCrypt hash authentication. The validation process is supposed to take about 20 ms at
        ///     the server side.
        /// </summary>
        [Test]
        public void CheckBCryptPerformance()
        {
            const int itterations = 50;

            const string salt = "saltsalt";
            const string password = "password";

            string passwordHash = PasswordHasher.GetKey(password, salt);

            for (int bcryptFactor = 7; bcryptFactor < 10; bcryptFactor++)
            {
                string bcryptHash = PasswordHasher.CreateDatabaseHash(passwordHash);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < itterations; i++)
                {
                    PasswordHasher.DatabaseAuthenticateUser(
                        passwordHash,
                        bcryptHash);
                }


                stopwatch.Stop();
                double elapsedTimePerHash = stopwatch.ElapsedMilliseconds/1.0/itterations;
                Console.WriteLine("Total execution time for verification with factor {1} per verification: {0} ms",
                    elapsedTimePerHash, 8);

                //Method is expected to take about 20 ms, so verify it takes more than 10 ms and less than 200 ms
                Assert.IsTrue(elapsedTimePerHash > 10, "The algorithm is too fast");
                Assert.IsTrue(elapsedTimePerHash < 200, "The algorithm is too slow");
            }
        }

        /// <summary>
        ///     Checks the performance of initial pin Pbkdf2 hashing for authentication. The validation process is supposed to take
        ///     about 20 ms at the client (totem / website) side.
        /// </summary>
        [Test]
        public void CheckWorkfactorPbkdf2Itterations()
        {
            const int itterationsI = 100;

            const string salt = "saltsalt";
            const string password = "password";


            for (int itterations = 0; itterations < 5; itterations++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 0; i < itterationsI; i++)
                {
                    string passwordHash = PasswordHasher.GetPinHashClientSide(password, salt);
                }

                stopwatch.Stop();

                double elapsedTimePerHash = stopwatch.ElapsedMilliseconds/1.0/itterationsI;

                Console.WriteLine(
                    "Total execution time for verification with 200 itterations per verification: {0} ms",
                    elapsedTimePerHash);

                //Method is expected to take about 20 ms, so verify it takes more than 5 ms and less than 80 ms
                Assert.IsTrue(elapsedTimePerHash > 5, "The algorithm is too fast");
                Assert.IsTrue(elapsedTimePerHash < 80, "The algorithm is too slow");
            }
        }

        [Test]
        public void CheckPbkdf2Output()
        {
            const string salt = "{60869A86-D153-4493-8AB4-778E58E92F04}";
            const string password = "password";

            string passwordHash = PasswordHasher.GetPinHashClientSide(password, salt);

            Console.WriteLine("Calculated Pbkdf2 hash: {0}", passwordHash);

            Assert.AreEqual("GeabvmKyaaAa5xFQvtrnHQkYUmFEgxmi7Fen1HSJF3gNrsoC0sPdrKMjgeutOd8uXt+9IhfA9DYrnvP9GGcICA4onR0OiZXgY4v14TpASdvI+zV6SnyIw2EG1kMYg1tcCxj99g4Gr8C6qhd6VVp7YsqWgxfzcgpFPqyqfF0IDrGLh9avRk69LcTw0EYD65Alm4u1hjGUaLzc9HxZhfxRqcdYbAuXxNJWOPEhDtBWz5YCoylrhWYS4AHmfVfnN6n1bB0+hTkoQrgwZJe0eaBt6KCzb8NGGprQ8cmZNz4VmsakkE5ofUQPL1yWVHhA8SY0VPa3cmn6Ln45Uqc1jrsraJEQdZzYrVgQ+ohlq2MxI8fnACPm8pcKER0Cl87GWlxFArIS+WPvllChQwNGFWvqZn7hUISyPe2GyxfWNeMjBGlB1dIrHhf1xR/peRoTPD3zuKKylqzZMMsfCgBwuz7LDiJqmVura+JPdaxC9GC4etCxnmE+9FyOSY63/c/f+Ka3K0Ba4ZO/pAj1v0fezqt/BX8Fv8Xgx3IbzXmfnOcSd8wGI/UiYtfLS3j6WN667F1a45KuQqJLNO93Bl0oasJNnwXwLODt6zYz8FByCngrhCrUhK7zJLzM10lv9FImFog9k0UXKTtWla8dHc/cDkmVby3B7rUtM/8lJoWyaUte+24=", passwordHash);
        }
    }
}