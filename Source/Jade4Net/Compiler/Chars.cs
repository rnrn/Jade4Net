namespace Jade.Compiler
{
    public class Chars
    {

        private Chars() { }

        /**
         * Special non-character used during error recovery. Signals that an illegal input character was skipped at this
         * input location.
         */
        public static readonly char DEL_ERROR = '\uFDEA';

        /**
         * Special non-character used during error recovery. Signals that the character at the following input location
         * was expected but not present in the input buffer.
         */
        public static readonly char INS_ERROR = '\uFDEB';

        /**
         * Special non-character used during error recovery. Signals that a rule resynchronization has to be performed
         * at the current input location.
         */
        public static readonly char RESYNC = '\uFDEC';

        /**
         * Special non-character used during error recovery. Signals that all characters up to the RESYNC_END
         * character need to be skipped as part of a resynchronization.
         */
        public static readonly char RESYNC_START = '\uFDED';

        /**
         * Special non-character used during error recovery. Signals the end of a resynchronization block.
         */
        public static readonly char RESYNC_END = '\uFDEE';

        /**
         * Special non-character used during error recovery. Signals a resynchronization at EOI.
         */
        public static readonly char RESYNC_EOI = '\uFDEF';

        /**
         * The End-of-Input non-character.
         */
        public static readonly char EOI = '\uFFFF';

        /**
         * Special non-character used by the {@link org.parboiled.buffers.IndentDedentInputBuffer}.
         */
        public static readonly char INDENT = '\uFDD0';

        /**
         * Special non-character used by the {@link org.parboiled.buffers.IndentDedentInputBuffer}.
         */
        public static readonly char DEDENT = '\uFDD1';

    }

}
