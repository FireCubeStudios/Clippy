using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace System.Runtime.CompilerServices
{
	/// <summary>
	/// Reserved to be used by the compiler for tracking metadata.
	/// This class should not be used by developers in source code.
	/// This dummy class is required to compile records when targeting .NET Standard 2.1
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class IsExternalInit
	{
	}
}
