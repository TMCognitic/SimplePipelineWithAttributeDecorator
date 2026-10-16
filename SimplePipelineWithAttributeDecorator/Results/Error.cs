namespace SimplePipelineWithAttributeDecorator.Results
{
    /// <summary>
    /// Décrit une erreur métier ou technique, identifiée par un code et un message.
    /// </summary>
    public record Error
    {
        /// <summary>
        /// Valeur représentant l'absence d'erreur (utilisée par un <see cref="Result"/> en succès).
        /// </summary>
        internal static Error None => new Error(Guid.Empty, string.Empty, string.Empty);

        /// <summary>
        /// Erreur générique représentant une exception survenue (le détail est censé être consigné dans les logs).
        /// </summary>
        internal static Error Exception => new Error();

        /// <summary>
        /// Crée une nouvelle erreur avec un identifiant unique généré.
        /// </summary>
        /// <param name="code">Code de l'erreur (non vide).</param>
        /// <param name="message">Message de l'erreur (non vide).</param>
        /// <returns>L'erreur créée.</returns>
        /// <exception cref="ArgumentException"><paramref name="code"/> ou <paramref name="message"/> est nul ou vide.</exception>
        public static Error Create(string code, string message)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);
            return new Error(Guid.NewGuid(), code, message);
        }

        /// <summary>
        /// Identifiant unique de l'erreur.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Code de l'erreur.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Message décrivant l'erreur.
        /// </summary>
        public string Message { get; }

        private Error(Guid id, string code, string message)
        {
            Id = id;
            Code = code;
            Message = message;
        }

        private Error()
        {
            Id = Guid.NewGuid();
            Code = "Exception";
            Message = $"An exception was thrown id : {Id.ToString()} (see log)";
        }
    }
}
