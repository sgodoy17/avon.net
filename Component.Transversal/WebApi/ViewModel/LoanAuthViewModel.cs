using System.ComponentModel.DataAnnotations;
using Component.Transversal.WebApi.Documentation;

namespace Component.Transversal.WebApi.ViewModel
{
    /// <summary>
    /// Request object to start validating a user.
    /// </summary>
    public class LoanAuthViewModel
    {
        /// <summary>
        /// Id of Totem who makes the request
        /// </summary>
        [Required]
        [SampleData(12)]
        public int TotemId { get; set; }

        /// <summary>
        /// Application specific identification number (row id) of user
        /// </summary>
        [Required]
        [SampleData(57)]
        public int UserId { get; set; }

        /// <summary>
        /// Hash of pincode, hashed using the <see cref="Component.Transversal.Cryptography.PasswordHasher.GetPinHashClientSide">GetPinHashClientSide</see> method.
        /// </summary>
        [Required]
        [SampleData("GeabvmKyaaAa5xFQvtrnHQkYUmFEgxmi7Fen1HSJF3gNrsoC0sPdrKMjgeutOd8uXt+9IhfA9DYrnvP9GGcICA4onR0OiZXgY4v14TpASdvI+zV6SnyIw2EG1kMYg1tcCxj99g4Gr8C6qhd6VVp7YsqWgxfzcgpFPqyqfF0IDrGLh9avRk69LcTw0EYD65Alm4u1hjGUaLzc9HxZhfxRqcdYbAuXxNJWOPEhDtBWz5YCoylrhWYS4AHmfVfnN6n1bB0+hTkoQrgwZJe0eaBt6KCzb8NGGprQ8cmZNz4VmsakkE5ofUQPL1yWVHhA8SY0VPa3cmn6Ln45Uqc1jrsraJEQdZzYrVgQ+ohlq2MxI8fnACPm8pcKER0Cl87GWlxFArIS+WPvllChQwNGFWvqZn7hUISyPe2GyxfWNeMjBGlB1dIrHhf1xR/peRoTPD3zuKKylqzZMMsfCgBwuz7LDiJqmVura+JPdaxC9GC4etCxnmE+9FyOSY63/c/f+Ka3K0Ba4ZO/pAj1v0fezqt/BX8Fv8Xgx3IbzXmfnOcSd8wGI/UiYtfLS3j6WN667F1a45KuQqJLNO93Bl0oasJNnwXwLODt6zYz8FByCngrhCrUhK7zJLzM10lv9FImFog9k0UXKTtWla8dHc/cDkmVby3B7rUtM/8lJoWyaUte+24=")]//For id 57, salt {60869A86-D153-4493-8AB4-778E58E92F04} and password "password"
        public string PinHash { get; set; }
    }
}