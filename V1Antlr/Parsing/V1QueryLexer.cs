//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.5
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from V1Query.g by ANTLR 4.5

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5")]
[System.CLSCompliant(false)]
public partial class V1QueryLexer : Lexer {
	public const int
		SINGLE_QUOTED_STRING=1, DOUBLE_QUOTED_STRING=2, CONTEXT_ASSET=3, VARIABLE_NAME=4, 
		NAME=5, OPEN_PAREN=6, CLOSE_PAREN=7, OPEN_BRACKET=8, CLOSE_BRACKET=9, 
		EQ=10, NE=11, LT=12, LTE=13, GT=14, GTE=15, PLUS=16, MINUS=17, HASH=18, 
		PIPE=19, AMP=20, SEMI=21, COLON=22, COMMA=23, DOT=24, DOT_AT=25;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"SINGLE_QUOTED_STRING", "DOUBLE_QUOTED_STRING", "CONTEXT_ASSET", "VARIABLE_NAME", 
		"NAME", "OPEN_PAREN", "CLOSE_PAREN", "OPEN_BRACKET", "CLOSE_BRACKET", 
		"EQ", "NE", "LT", "LTE", "GT", "GTE", "PLUS", "MINUS", "HASH", "PIPE", 
		"AMP", "SEMI", "COLON", "COMMA", "DOT", "DOT_AT", "ASCII_VISIBLE", "NAME_CHAR", 
		"ALPHA", "DIGIT", "SYMBOL"
	};


	public V1QueryLexer(ICharStream input)
		: base(input)
	{
		Interpreter = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, "'$'", null, null, "'('", "')'", "'['", "']'", "'='", 
		"'!='", "'<'", "'<='", "'>'", "'>='", "'+'", "'-'", "'#'", "'|'", "'&'", 
		"';'", "':'", "','", "'.'", "'.@'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "SINGLE_QUOTED_STRING", "DOUBLE_QUOTED_STRING", "CONTEXT_ASSET", 
		"VARIABLE_NAME", "NAME", "OPEN_PAREN", "CLOSE_PAREN", "OPEN_BRACKET", 
		"CLOSE_BRACKET", "EQ", "NE", "LT", "LTE", "GT", "GTE", "PLUS", "MINUS", 
		"HASH", "PIPE", "AMP", "SEMI", "COLON", "COMMA", "DOT", "DOT_AT"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "V1Query.g"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	public static readonly string _serializedATN =
		"\x3\x430\xD6D1\x8206\xAD2D\x4417\xAEF1\x8D80\xAADD\x2\x1B\x97\b\x1\x4"+
		"\x2\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b"+
		"\x4\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x4\r\t\r\x4\xE\t\xE\x4\xF\t\xF\x4"+
		"\x10\t\x10\x4\x11\t\x11\x4\x12\t\x12\x4\x13\t\x13\x4\x14\t\x14\x4\x15"+
		"\t\x15\x4\x16\t\x16\x4\x17\t\x17\x4\x18\t\x18\x4\x19\t\x19\x4\x1A\t\x1A"+
		"\x4\x1B\t\x1B\x4\x1C\t\x1C\x4\x1D\t\x1D\x4\x1E\t\x1E\x4\x1F\t\x1F\x3\x2"+
		"\x3\x2\a\x2\x42\n\x2\f\x2\xE\x2\x45\v\x2\x3\x2\x3\x2\x3\x3\x3\x3\a\x3"+
		"K\n\x3\f\x3\xE\x3N\v\x3\x3\x3\x3\x3\x3\x4\x3\x4\x3\x5\x3\x5\x3\x5\x3\x6"+
		"\x6\x6X\n\x6\r\x6\xE\x6Y\x3\a\x3\a\x3\b\x3\b\x3\t\x3\t\x3\n\x3\n\x3\v"+
		"\x3\v\x3\f\x3\f\x3\f\x3\r\x3\r\x3\xE\x3\xE\x3\xE\x3\xF\x3\xF\x3\x10\x3"+
		"\x10\x3\x10\x3\x11\x3\x11\x3\x12\x3\x12\x3\x13\x3\x13\x3\x14\x3\x14\x3"+
		"\x15\x3\x15\x3\x16\x3\x16\x3\x17\x3\x17\x3\x18\x3\x18\x3\x19\x3\x19\x3"+
		"\x1A\x3\x1A\x3\x1A\x3\x1B\x3\x1B\x3\x1B\x5\x1B\x8B\n\x1B\x3\x1C\x3\x1C"+
		"\x3\x1C\x5\x1C\x90\n\x1C\x3\x1D\x3\x1D\x3\x1E\x3\x1E\x3\x1F\x3\x1F\x2"+
		"\x2 \x3\x3\x5\x4\a\x5\t\x6\v\a\r\b\xF\t\x11\n\x13\v\x15\f\x17\r\x19\xE"+
		"\x1B\xF\x1D\x10\x1F\x11!\x12#\x13%\x14\'\x15)\x16+\x17-\x18/\x19\x31\x1A"+
		"\x33\x1B\x35\x2\x37\x2\x39\x2;\x2=\x2\x3\x2\x6\x3\x2))\x3\x2$$\x4\x2\x43"+
		"\\\x63|\x6\x2#\x31<\x42]\x62}\x80\x98\x2\x3\x3\x2\x2\x2\x2\x5\x3\x2\x2"+
		"\x2\x2\a\x3\x2\x2\x2\x2\t\x3\x2\x2\x2\x2\v\x3\x2\x2\x2\x2\r\x3\x2\x2\x2"+
		"\x2\xF\x3\x2\x2\x2\x2\x11\x3\x2\x2\x2\x2\x13\x3\x2\x2\x2\x2\x15\x3\x2"+
		"\x2\x2\x2\x17\x3\x2\x2\x2\x2\x19\x3\x2\x2\x2\x2\x1B\x3\x2\x2\x2\x2\x1D"+
		"\x3\x2\x2\x2\x2\x1F\x3\x2\x2\x2\x2!\x3\x2\x2\x2\x2#\x3\x2\x2\x2\x2%\x3"+
		"\x2\x2\x2\x2\'\x3\x2\x2\x2\x2)\x3\x2\x2\x2\x2+\x3\x2\x2\x2\x2-\x3\x2\x2"+
		"\x2\x2/\x3\x2\x2\x2\x2\x31\x3\x2\x2\x2\x2\x33\x3\x2\x2\x2\x3?\x3\x2\x2"+
		"\x2\x5H\x3\x2\x2\x2\aQ\x3\x2\x2\x2\tS\x3\x2\x2\x2\vW\x3\x2\x2\x2\r[\x3"+
		"\x2\x2\x2\xF]\x3\x2\x2\x2\x11_\x3\x2\x2\x2\x13\x61\x3\x2\x2\x2\x15\x63"+
		"\x3\x2\x2\x2\x17\x65\x3\x2\x2\x2\x19h\x3\x2\x2\x2\x1Bj\x3\x2\x2\x2\x1D"+
		"m\x3\x2\x2\x2\x1Fo\x3\x2\x2\x2!r\x3\x2\x2\x2#t\x3\x2\x2\x2%v\x3\x2\x2"+
		"\x2\'x\x3\x2\x2\x2)z\x3\x2\x2\x2+|\x3\x2\x2\x2-~\x3\x2\x2\x2/\x80\x3\x2"+
		"\x2\x2\x31\x82\x3\x2\x2\x2\x33\x84\x3\x2\x2\x2\x35\x8A\x3\x2\x2\x2\x37"+
		"\x8F\x3\x2\x2\x2\x39\x91\x3\x2\x2\x2;\x93\x3\x2\x2\x2=\x95\x3\x2\x2\x2"+
		"?\x43\a)\x2\x2@\x42\n\x2\x2\x2\x41@\x3\x2\x2\x2\x42\x45\x3\x2\x2\x2\x43"+
		"\x41\x3\x2\x2\x2\x43\x44\x3\x2\x2\x2\x44\x46\x3\x2\x2\x2\x45\x43\x3\x2"+
		"\x2\x2\x46G\a)\x2\x2G\x4\x3\x2\x2\x2HL\a$\x2\x2IK\n\x3\x2\x2JI\x3\x2\x2"+
		"\x2KN\x3\x2\x2\x2LJ\x3\x2\x2\x2LM\x3\x2\x2\x2MO\x3\x2\x2\x2NL\x3\x2\x2"+
		"\x2OP\a$\x2\x2P\x6\x3\x2\x2\x2QR\a&\x2\x2R\b\x3\x2\x2\x2ST\a&\x2\x2TU"+
		"\x5\v\x6\x2U\n\x3\x2\x2\x2VX\x5\x37\x1C\x2WV\x3\x2\x2\x2XY\x3\x2\x2\x2"+
		"YW\x3\x2\x2\x2YZ\x3\x2\x2\x2Z\f\x3\x2\x2\x2[\\\a*\x2\x2\\\xE\x3\x2\x2"+
		"\x2]^\a+\x2\x2^\x10\x3\x2\x2\x2_`\a]\x2\x2`\x12\x3\x2\x2\x2\x61\x62\a"+
		"_\x2\x2\x62\x14\x3\x2\x2\x2\x63\x64\a?\x2\x2\x64\x16\x3\x2\x2\x2\x65\x66"+
		"\a#\x2\x2\x66g\a?\x2\x2g\x18\x3\x2\x2\x2hi\a>\x2\x2i\x1A\x3\x2\x2\x2j"+
		"k\a>\x2\x2kl\a?\x2\x2l\x1C\x3\x2\x2\x2mn\a@\x2\x2n\x1E\x3\x2\x2\x2op\a"+
		"@\x2\x2pq\a?\x2\x2q \x3\x2\x2\x2rs\a-\x2\x2s\"\x3\x2\x2\x2tu\a/\x2\x2"+
		"u$\x3\x2\x2\x2vw\a%\x2\x2w&\x3\x2\x2\x2xy\a~\x2\x2y(\x3\x2\x2\x2z{\a("+
		"\x2\x2{*\x3\x2\x2\x2|}\a=\x2\x2},\x3\x2\x2\x2~\x7F\a<\x2\x2\x7F.\x3\x2"+
		"\x2\x2\x80\x81\a.\x2\x2\x81\x30\x3\x2\x2\x2\x82\x83\a\x30\x2\x2\x83\x32"+
		"\x3\x2\x2\x2\x84\x85\a\x30\x2\x2\x85\x86\a\x42\x2\x2\x86\x34\x3\x2\x2"+
		"\x2\x87\x8B\x5\x39\x1D\x2\x88\x8B\x5;\x1E\x2\x89\x8B\x5=\x1F\x2\x8A\x87"+
		"\x3\x2\x2\x2\x8A\x88\x3\x2\x2\x2\x8A\x89\x3\x2\x2\x2\x8B\x36\x3\x2\x2"+
		"\x2\x8C\x90\x5\x39\x1D\x2\x8D\x90\x5;\x1E\x2\x8E\x90\a\x61\x2\x2\x8F\x8C"+
		"\x3\x2\x2\x2\x8F\x8D\x3\x2\x2\x2\x8F\x8E\x3\x2\x2\x2\x90\x38\x3\x2\x2"+
		"\x2\x91\x92\t\x4\x2\x2\x92:\x3\x2\x2\x2\x93\x94\x4\x32;\x2\x94<\x3\x2"+
		"\x2\x2\x95\x96\t\x5\x2\x2\x96>\x3\x2\x2\x2\b\x2\x43LY\x8A\x8F\x2";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
