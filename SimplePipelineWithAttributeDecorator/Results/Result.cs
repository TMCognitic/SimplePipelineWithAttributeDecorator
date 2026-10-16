namespace SimplePipelineWithAttributeDecorator.Results
{
    /// <summary>
    /// Résultat d'une opération : succès, ou échec accompagné d'une <see cref="Results.Error"/>.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Convertit implicitement une <see cref="Results.Error"/> en <see cref="Result"/> en échec.
        /// </summary>
        /// <param name="error">Erreur à encapsuler.</param>
        public static implicit operator Result(Error error)
        {
            return Failure(error);
        }

        /// <summary>
        /// Convertit explicitement une <see cref="Exception"/> en <see cref="Result"/> en échec
        /// (l'erreur générique <c>Exception</c> est utilisée, le détail de l'exception n'est pas conservé).
        /// </summary>
        /// <param name="exception">Exception survenue.</param>
        public static explicit operator Result(Exception exception)
        {
            return Failure(Error.Exception);
        }

        /// <summary>
        /// Crée un résultat en succès.
        /// </summary>
        /// <returns>Un <see cref="Result"/> en succès.</returns>
        public static Result Success()
        {
            return new Result(true, Error.None);
        }

        /// <summary>
        /// Crée un résultat en échec.
        /// </summary>
        /// <param name="error">Erreur associée (code et message non vides).</param>
        /// <returns>Un <see cref="Result"/> en échec.</returns>
        /// <exception cref="ArgumentException">Le code ou le message de l'erreur est nul ou vide.</exception>
        public static Result Failure(Error error)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(error.Code, "error.Code");
            ArgumentException.ThrowIfNullOrWhiteSpace(error.Message, "error.Message");
            return new Result(false, error);
        }

        /// <summary>
        /// Indique si l'opération a réussi.
        /// </summary>
        public bool IsSuccess
        {
            get;
        }

        /// <summary>
        /// Indique si l'opération a échoué.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Erreur associée au résultat (valeur « aucune erreur » en cas de succès).
        /// </summary>
        public Error Error
        {
            get;
        }

        internal Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
    }
}
