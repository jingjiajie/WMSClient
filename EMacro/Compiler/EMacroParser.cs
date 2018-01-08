//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from EMacro.g4 by ANTLR 4.7

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public partial class EMacroParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		TEXT_SHARP=1, TEXT_AT=2, TEXT=3, WS=4, COMMAND_SHARP=5, REPEAT=6, AREA=7, 
		VAR=8, IN=9, WRITE=10, SET=11, COLOR=12, DIGIT=13, ID=14, JS_SHARP=15, 
		JS_AT=16, JSTEXT=17;
	public const int
		RULE_stat = 0, RULE_command = 1, RULE_text = 2, RULE_commandStartSharp = 3, 
		RULE_commandStartAt = 4, RULE_writeCommand = 5, RULE_repeatCommand = 6, 
		RULE_setColorCommand = 7, RULE_jsCommand = 8;
	public static readonly string[] ruleNames = {
		"stat", "command", "text", "commandStartSharp", "commandStartAt", "writeCommand", 
		"repeatCommand", "setColorCommand", "jsCommand"
	};

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, null, "'REPEAT'", "'AREA'", "'VAR'", "'IN'", 
		"'WRITE'", "'SET'", "'COLOR'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "TEXT_SHARP", "TEXT_AT", "TEXT", "WS", "COMMAND_SHARP", "REPEAT", 
		"AREA", "VAR", "IN", "WRITE", "SET", "COLOR", "DIGIT", "ID", "JS_SHARP", 
		"JS_AT", "JSTEXT"
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

	public override string GrammarFileName { get { return "EMacro.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static EMacroParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public EMacroParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public EMacroParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}
	public partial class StatContext : ParserRuleContext {
		public TextContext text() {
			return GetRuleContext<TextContext>(0);
		}
		public CommandContext[] command() {
			return GetRuleContexts<CommandContext>();
		}
		public CommandContext command(int i) {
			return GetRuleContext<CommandContext>(i);
		}
		public StatContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_stat; } }
		public override void EnterRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.EnterStat(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.ExitStat(this);
		}
	}

	[RuleVersion(0)]
	public StatContext stat() {
		StatContext _localctx = new StatContext(Context, State);
		EnterRule(_localctx, 0, RULE_stat);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 19;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==TEXT) {
				{
				State = 18; text();
				}
			}

			State = 24;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << TEXT_SHARP) | (1L << TEXT_AT) | (1L << COMMAND_SHARP) | (1L << JS_SHARP) | (1L << JS_AT))) != 0)) {
				{
				{
				State = 21; command();
				}
				}
				State = 26;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class CommandContext : ParserRuleContext {
		public WriteCommandContext writeCommand() {
			return GetRuleContext<WriteCommandContext>(0);
		}
		public RepeatCommandContext repeatCommand() {
			return GetRuleContext<RepeatCommandContext>(0);
		}
		public SetColorCommandContext setColorCommand() {
			return GetRuleContext<SetColorCommandContext>(0);
		}
		public CommandContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_command; } }
		public override void EnterRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.EnterCommand(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.ExitCommand(this);
		}
	}

	[RuleVersion(0)]
	public CommandContext command() {
		CommandContext _localctx = new CommandContext(Context, State);
		EnterRule(_localctx, 2, RULE_command);
		try {
			State = 30;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,2,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 27; writeCommand();
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 28; repeatCommand();
				}
				break;
			case 3:
				EnterOuterAlt(_localctx, 3);
				{
				State = 29; setColorCommand();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class TextContext : ParserRuleContext {
		public ITerminalNode TEXT() { return GetToken(EMacroParser.TEXT, 0); }
		public TextContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_text; } }
		public override void EnterRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.EnterText(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.ExitText(this);
		}
	}

	[RuleVersion(0)]
	public TextContext text() {
		TextContext _localctx = new TextContext(Context, State);
		EnterRule(_localctx, 4, RULE_text);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 32; Match(TEXT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class CommandStartSharpContext : ParserRuleContext {
		public ITerminalNode TEXT_SHARP() { return GetToken(EMacroParser.TEXT_SHARP, 0); }
		public ITerminalNode JS_SHARP() { return GetToken(EMacroParser.JS_SHARP, 0); }
		public ITerminalNode COMMAND_SHARP() { return GetToken(EMacroParser.COMMAND_SHARP, 0); }
		public CommandStartSharpContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_commandStartSharp; } }
		public override void EnterRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.EnterCommandStartSharp(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.ExitCommandStartSharp(this);
		}
	}

	[RuleVersion(0)]
	public CommandStartSharpContext commandStartSharp() {
		CommandStartSharpContext _localctx = new CommandStartSharpContext(Context, State);
		EnterRule(_localctx, 6, RULE_commandStartSharp);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 34;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << TEXT_SHARP) | (1L << COMMAND_SHARP) | (1L << JS_SHARP))) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class CommandStartAtContext : ParserRuleContext {
		public ITerminalNode TEXT_AT() { return GetToken(EMacroParser.TEXT_AT, 0); }
		public ITerminalNode JS_AT() { return GetToken(EMacroParser.JS_AT, 0); }
		public CommandStartAtContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_commandStartAt; } }
		public override void EnterRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.EnterCommandStartAt(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.ExitCommandStartAt(this);
		}
	}

	[RuleVersion(0)]
	public CommandStartAtContext commandStartAt() {
		CommandStartAtContext _localctx = new CommandStartAtContext(Context, State);
		EnterRule(_localctx, 8, RULE_commandStartAt);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 36;
			_la = TokenStream.LA(1);
			if ( !(_la==TEXT_AT || _la==JS_AT) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class WriteCommandContext : ParserRuleContext {
		public CommandStartSharpContext commandStartSharp() {
			return GetRuleContext<CommandStartSharpContext>(0);
		}
		public ITerminalNode WRITE() { return GetToken(EMacroParser.WRITE, 0); }
		public JsCommandContext jsCommand() {
			return GetRuleContext<JsCommandContext>(0);
		}
		public CommandStartAtContext commandStartAt() {
			return GetRuleContext<CommandStartAtContext>(0);
		}
		public WriteCommandContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_writeCommand; } }
		public override void EnterRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.EnterWriteCommand(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.ExitWriteCommand(this);
		}
	}

	[RuleVersion(0)]
	public WriteCommandContext writeCommand() {
		WriteCommandContext _localctx = new WriteCommandContext(Context, State);
		EnterRule(_localctx, 10, RULE_writeCommand);
		try {
			State = 45;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case TEXT_SHARP:
			case COMMAND_SHARP:
			case JS_SHARP:
				EnterOuterAlt(_localctx, 1);
				{
				State = 38; commandStartSharp();
				State = 39; Match(WRITE);
				State = 40; jsCommand();
				}
				break;
			case TEXT_AT:
			case JS_AT:
				EnterOuterAlt(_localctx, 2);
				{
				State = 42; commandStartAt();
				State = 43; jsCommand();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class RepeatCommandContext : ParserRuleContext {
		public CommandStartSharpContext commandStartSharp() {
			return GetRuleContext<CommandStartSharpContext>(0);
		}
		public ITerminalNode REPEAT() { return GetToken(EMacroParser.REPEAT, 0); }
		public ITerminalNode AREA() { return GetToken(EMacroParser.AREA, 0); }
		public ITerminalNode[] DIGIT() { return GetTokens(EMacroParser.DIGIT); }
		public ITerminalNode DIGIT(int i) {
			return GetToken(EMacroParser.DIGIT, i);
		}
		public ITerminalNode VAR() { return GetToken(EMacroParser.VAR, 0); }
		public ITerminalNode ID() { return GetToken(EMacroParser.ID, 0); }
		public ITerminalNode IN() { return GetToken(EMacroParser.IN, 0); }
		public JsCommandContext jsCommand() {
			return GetRuleContext<JsCommandContext>(0);
		}
		public RepeatCommandContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_repeatCommand; } }
		public override void EnterRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.EnterRepeatCommand(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.ExitRepeatCommand(this);
		}
	}

	[RuleVersion(0)]
	public RepeatCommandContext repeatCommand() {
		RepeatCommandContext _localctx = new RepeatCommandContext(Context, State);
		EnterRule(_localctx, 12, RULE_repeatCommand);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 47; commandStartSharp();
			State = 48; Match(REPEAT);
			State = 49; Match(AREA);
			State = 50; Match(DIGIT);
			State = 51; Match(DIGIT);
			State = 52; Match(VAR);
			State = 53; Match(ID);
			State = 54; Match(IN);
			State = 55; jsCommand();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class SetColorCommandContext : ParserRuleContext {
		public CommandStartSharpContext commandStartSharp() {
			return GetRuleContext<CommandStartSharpContext>(0);
		}
		public ITerminalNode SET() { return GetToken(EMacroParser.SET, 0); }
		public ITerminalNode COLOR() { return GetToken(EMacroParser.COLOR, 0); }
		public JsCommandContext jsCommand() {
			return GetRuleContext<JsCommandContext>(0);
		}
		public SetColorCommandContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_setColorCommand; } }
		public override void EnterRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.EnterSetColorCommand(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.ExitSetColorCommand(this);
		}
	}

	[RuleVersion(0)]
	public SetColorCommandContext setColorCommand() {
		SetColorCommandContext _localctx = new SetColorCommandContext(Context, State);
		EnterRule(_localctx, 14, RULE_setColorCommand);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 57; commandStartSharp();
			State = 58; Match(SET);
			State = 59; Match(COLOR);
			State = 60; jsCommand();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class JsCommandContext : ParserRuleContext {
		public ITerminalNode JSTEXT() { return GetToken(EMacroParser.JSTEXT, 0); }
		public JsCommandContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_jsCommand; } }
		public override void EnterRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.EnterJsCommand(this);
		}
		public override void ExitRule(IParseTreeListener listener) {
			IEMacroListener typedListener = listener as IEMacroListener;
			if (typedListener != null) typedListener.ExitJsCommand(this);
		}
	}

	[RuleVersion(0)]
	public JsCommandContext jsCommand() {
		JsCommandContext _localctx = new JsCommandContext(Context, State);
		EnterRule(_localctx, 16, RULE_jsCommand);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 62; Match(JSTEXT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x3', '\x13', '\x43', '\x4', '\x2', '\t', '\x2', '\x4', '\x3', 
		'\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', '\x5', '\x4', 
		'\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', '\t', '\b', 
		'\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x3', '\x2', '\x5', 
		'\x2', '\x16', '\n', '\x2', '\x3', '\x2', '\a', '\x2', '\x19', '\n', '\x2', 
		'\f', '\x2', '\xE', '\x2', '\x1C', '\v', '\x2', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x5', '\x3', '!', '\n', '\x3', '\x3', '\x4', '\x3', '\x4', 
		'\x3', '\x5', '\x3', '\x5', '\x3', '\x6', '\x3', '\x6', '\x3', '\a', '\x3', 
		'\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\a', 
		'\x5', '\a', '\x30', '\n', '\a', '\x3', '\b', '\x3', '\b', '\x3', '\b', 
		'\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', 
		'\b', '\x3', '\b', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', 
		'\x3', '\t', '\x3', '\n', '\x3', '\n', '\x3', '\n', '\x2', '\x2', '\v', 
		'\x2', '\x4', '\x6', '\b', '\n', '\f', '\xE', '\x10', '\x12', '\x2', '\x4', 
		'\x5', '\x2', '\x3', '\x3', '\a', '\a', '\x11', '\x11', '\x4', '\x2', 
		'\x4', '\x4', '\x12', '\x12', '\x2', '>', '\x2', '\x15', '\x3', '\x2', 
		'\x2', '\x2', '\x4', ' ', '\x3', '\x2', '\x2', '\x2', '\x6', '\"', '\x3', 
		'\x2', '\x2', '\x2', '\b', '$', '\x3', '\x2', '\x2', '\x2', '\n', '&', 
		'\x3', '\x2', '\x2', '\x2', '\f', '/', '\x3', '\x2', '\x2', '\x2', '\xE', 
		'\x31', '\x3', '\x2', '\x2', '\x2', '\x10', ';', '\x3', '\x2', '\x2', 
		'\x2', '\x12', '@', '\x3', '\x2', '\x2', '\x2', '\x14', '\x16', '\x5', 
		'\x6', '\x4', '\x2', '\x15', '\x14', '\x3', '\x2', '\x2', '\x2', '\x15', 
		'\x16', '\x3', '\x2', '\x2', '\x2', '\x16', '\x1A', '\x3', '\x2', '\x2', 
		'\x2', '\x17', '\x19', '\x5', '\x4', '\x3', '\x2', '\x18', '\x17', '\x3', 
		'\x2', '\x2', '\x2', '\x19', '\x1C', '\x3', '\x2', '\x2', '\x2', '\x1A', 
		'\x18', '\x3', '\x2', '\x2', '\x2', '\x1A', '\x1B', '\x3', '\x2', '\x2', 
		'\x2', '\x1B', '\x3', '\x3', '\x2', '\x2', '\x2', '\x1C', '\x1A', '\x3', 
		'\x2', '\x2', '\x2', '\x1D', '!', '\x5', '\f', '\a', '\x2', '\x1E', '!', 
		'\x5', '\xE', '\b', '\x2', '\x1F', '!', '\x5', '\x10', '\t', '\x2', ' ', 
		'\x1D', '\x3', '\x2', '\x2', '\x2', ' ', '\x1E', '\x3', '\x2', '\x2', 
		'\x2', ' ', '\x1F', '\x3', '\x2', '\x2', '\x2', '!', '\x5', '\x3', '\x2', 
		'\x2', '\x2', '\"', '#', '\a', '\x5', '\x2', '\x2', '#', '\a', '\x3', 
		'\x2', '\x2', '\x2', '$', '%', '\t', '\x2', '\x2', '\x2', '%', '\t', '\x3', 
		'\x2', '\x2', '\x2', '&', '\'', '\t', '\x3', '\x2', '\x2', '\'', '\v', 
		'\x3', '\x2', '\x2', '\x2', '(', ')', '\x5', '\b', '\x5', '\x2', ')', 
		'*', '\a', '\f', '\x2', '\x2', '*', '+', '\x5', '\x12', '\n', '\x2', '+', 
		'\x30', '\x3', '\x2', '\x2', '\x2', ',', '-', '\x5', '\n', '\x6', '\x2', 
		'-', '.', '\x5', '\x12', '\n', '\x2', '.', '\x30', '\x3', '\x2', '\x2', 
		'\x2', '/', '(', '\x3', '\x2', '\x2', '\x2', '/', ',', '\x3', '\x2', '\x2', 
		'\x2', '\x30', '\r', '\x3', '\x2', '\x2', '\x2', '\x31', '\x32', '\x5', 
		'\b', '\x5', '\x2', '\x32', '\x33', '\a', '\b', '\x2', '\x2', '\x33', 
		'\x34', '\a', '\t', '\x2', '\x2', '\x34', '\x35', '\a', '\xF', '\x2', 
		'\x2', '\x35', '\x36', '\a', '\xF', '\x2', '\x2', '\x36', '\x37', '\a', 
		'\n', '\x2', '\x2', '\x37', '\x38', '\a', '\x10', '\x2', '\x2', '\x38', 
		'\x39', '\a', '\v', '\x2', '\x2', '\x39', ':', '\x5', '\x12', '\n', '\x2', 
		':', '\xF', '\x3', '\x2', '\x2', '\x2', ';', '<', '\x5', '\b', '\x5', 
		'\x2', '<', '=', '\a', '\r', '\x2', '\x2', '=', '>', '\a', '\xE', '\x2', 
		'\x2', '>', '?', '\x5', '\x12', '\n', '\x2', '?', '\x11', '\x3', '\x2', 
		'\x2', '\x2', '@', '\x41', '\a', '\x13', '\x2', '\x2', '\x41', '\x13', 
		'\x3', '\x2', '\x2', '\x2', '\x6', '\x15', '\x1A', ' ', '/',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}