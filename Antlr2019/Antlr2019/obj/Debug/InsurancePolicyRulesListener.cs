//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.6
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Bhabesh\Programming\Antlr2019\Antlr2019\InsurancePolicyRules.g4 by ANTLR 4.6.6

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Antlr2019 {
using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="InsurancePolicyRulesParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.6")]
[System.CLSCompliant(false)]
public interface IInsurancePolicyRulesListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="InsurancePolicyRulesParser.csvFile"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCsvFile([NotNull] InsurancePolicyRulesParser.CsvFileContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="InsurancePolicyRulesParser.csvFile"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCsvFile([NotNull] InsurancePolicyRulesParser.CsvFileContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="InsurancePolicyRulesParser.hdr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterHdr([NotNull] InsurancePolicyRulesParser.HdrContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="InsurancePolicyRulesParser.hdr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitHdr([NotNull] InsurancePolicyRulesParser.HdrContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="InsurancePolicyRulesParser.row"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRow([NotNull] InsurancePolicyRulesParser.RowContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="InsurancePolicyRulesParser.row"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRow([NotNull] InsurancePolicyRulesParser.RowContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="InsurancePolicyRulesParser.field"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterField([NotNull] InsurancePolicyRulesParser.FieldContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="InsurancePolicyRulesParser.field"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitField([NotNull] InsurancePolicyRulesParser.FieldContext context);
}
} // namespace Antlr2019
