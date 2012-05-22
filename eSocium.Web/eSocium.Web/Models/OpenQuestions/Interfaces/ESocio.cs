using System;
using System.Collections;
using System.Collections.Generic;

namespace eSocio
{
    // Interface members are always public
    // IEquatable(Of T) should be implemented for any type that might be stored in a generic collection.
    // If you implement IEquatable(Of T), you should also override the base class implementations of Object.Equals(Object) and GetHashCode so that their behavior is consistent with that of the IEquatable(Of T).Equals method.
    // static operator == and operator != may be defined in an implementation, but this is not typical in .NET
    // Interfaces cannot contain operators

    public struct Pair<T1, T2> : IEquatable<Pair<T1, T2>>
    {
        public Pair(T1 t1, T2 t2)
            : this()
        {
            First = t1;
            Second = t2;
        }
        public T1 First { get; set; }
        public T2 Second { get; set; }

        public override int GetHashCode()
        {
            return First.GetHashCode() ^ Second.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Pair<T1, T2>))
                return false;
            Pair<T1, T2> rhs = (Pair<T1, T2>)obj;
            return First.Equals(rhs.First) && Second.Equals(rhs.Second);
        }

        public bool Equals(Pair<T1, T2> other)
        {
            return First.Equals(other.First) && Second.Equals(other.Second);
        }

        public static bool operator ==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
        {
            return !lhs.Equals(rhs);
        }
    }

    namespace PollData
    {
        public interface IRespondent : IEquatable<IRespondent>
        {
            int Id { get; }
            string Name { get; }
            string At(int QuestionId);
        }

        public interface IQuestion : IEquatable<IQuestion>
        {
            int Id { get; }
            string Wording { get; }
            string At(int RespondentId);
        }

        public interface IPoll
        {
            IList<IQuestion> Questions { get; }
            IList<IRespondent> Respondents { get; }
            string At(int QuestionId, int RespondentId);
        }
    }

    namespace NLP
    {
        /// <summary>
        /// Represents a part of speech.
        /// </summary>
        public interface IPOS : IEquatable<IPOS>
        {
        }

        /// <summary>
        /// Represents a paradigm. There may be different POS in a paradigm.
        /// </summary>
        public interface IParadigm : IEquatable<IParadigm>
        {
            IList<IForm> Forms { get; }
            IForm InitialForm { get; }
        }

        /// <summary>
        /// Represents a form.
        /// </summary>
        public interface IForm : IEquatable<IForm>
        {
            IParadigm Paradigm { get; }
            IPOS POS { get; }
            string Prefix { get; }
            string Suffix { get; }
            string DecorateStem(string stem);
            string DecorateLemma(ILemma lemma);
        }

        /// <summary>
        /// Represents a lemma. There may be different POS in a lemma.
        /// </summary>
        public interface ILemma : IEquatable<ILemma>
        {
            /// <summary>
            /// Gets the stem of the lemma.
            /// </summary>
            string Stem { get; }
            /// <summary>
            /// Gets the paradigm of the lemma.
            /// </summary>
            IParadigm Paradigm { get; }
        }

        /// <summary>
        /// Defines methods to split a text into words or sentences.
        /// </summary>
        public interface IText2WordSplitter
        {
            /// <summary>
            /// Splits an arbitrary text into words.
            /// </summary>
            /// <param name="text">The input text to be split.</param>
            /// <returns>The list of words.</returns>
            IList<string> SplitTextIntoWords(string text);
            IList<string> SplitIncompleteTextIntoWords(string text);
            IList<string> SplitCompleteTextIntoWords(string text);
            IList<string> SplitCompleteTextIntoSentences(string text);
            IList<string> SplitSentenceIntoWords(string text);
        }

        /// <summary>
        /// Defines methods to split a text into lemmas.
        /// </summary>
        public interface IText2LemmaSplitter
        {
            /// <summary>
            /// Splits an arbitrary text into lemmas.
            /// </summary>
            /// <param name="text">The input text to be split.</param>
            /// <returns>The list of list of lemmas.</returns>
            IList<IList<Pair<ILemma, IForm>>> SplitTextIntoWords(string text);
            IList<IList<Pair<ILemma, IForm>>> SplitIncompleteTextIntoWords(string text);
            IList<IList<Pair<ILemma, IForm>>> SplitCompleteTextIntoWords(string text);
            IList<IList<Pair<ILemma, IForm>>> SplitSentenceIntoWords(string text);
        }

        /// <summary>
        /// Defines methods to normalize words.
        /// </summary>
        public interface IWordNormalizer
        {
            /// <summary>
            /// Returns all the possible lemmas.
            /// </summary>
            /// <param name="word">A word.</param>
            /// <returns>An unordered list of all the possible lemmas.</returns>
            IList<ILemma> NormalizeWord(string word);
            /// <summary>
            /// Returns the normal form of the lemma.
            /// </summary>
            /// <param name="lemma">A lemma.</param>
            /// <returns>A word in the normal form.</returns>
            string GetNormalWord(ILemma lemma);
        }
    }

    namespace Arrays
    {
        public interface IVector
        {
            int Size { get; }
        }

        public interface ISparseVector<T> : IVector, IEnumerable<KeyValuePair<int, T>>, IEnumerable
        {
            T DefaultValue { get; }
        }

        public interface IDenseVector<T> : IVector
        {
            T At(int i);
        }

        public interface IMatrix
        {
            int Size1 { get; }
            int Size2 { get; }
        }

        public interface ISparseMatrix<T> : IMatrix, IEnumerable<KeyValuePair<Pair<int, int>, T>>, IEnumerable
        {
            T DefaultValue { get; }
        }

        public interface IDenseMatrix<T> : IMatrix
        {
            T At(int i, int j);
        }
    }
}