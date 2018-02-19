﻿' Copyright (c) Microsoft Corporation.  All rights reserved.
'The following code was generated by Microsoft Visual Studio 2005.
'The test owner should check each test for validity.
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports System.Text
Imports System.Collections.Generic
Imports PInvoke
Imports PInvoke.Transform
Imports PInvokeTest
Imports System.CodeDom
Imports CodeTransform_Acc = PInvokeTest.PInvoke_Transform_CodeTransformAccessor

'''<summary>
'''This is a test class for PInvoke.Transform.CodeTransform and is intended
'''to contain all PInvoke.Transform.CodeTransform Unit Tests
'''</summary>
<TestClass()> _
Public Class CodeTransformTest
    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = value
        End Set
    End Property

    ''' <summary>
    ''' Basic expressions.  These are natively supported by the codedom
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub TryGenExpression1()
        VerifyExpression("1+1", "(1 + 1)")
        VerifyExpression("1-1", "(1 - 1)")
        VerifyExpression("1/1", "(1 / 1)")
        VerifyExpression("1*1", "(1 * 1)")
    End Sub

    ''' <summary>
    ''' Generate !/Not expressions which are not natively supported by the CodeDom 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub TryGenNot()
        VerifyExpression(LanguageType.VisualBasic, "!1", "Not (1)")
        VerifyExpression(LanguageType.CSharp, "!1", "! (1)")
    End Sub

    <TestMethod()> _
    Public Sub TryGenShift()
        VerifyExpression("1<<1", "(1) << (1)")
        VerifyExpression("1<< 4+2", "(1) << ((4 + 2))")
    End Sub

    ''' <summary>
    ''' Generate a binary | expression
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub TryGenBinaryOr()
        VerifyExpression("1|1", "(1 Or 1)")
    End Sub

    ''' <summary>
    ''' Generate a binary and expression
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub TryGenBinaryAnd()
        VerifyExpression("42&3", "(42 And 3)")
    End Sub


    ''' <summary>
    ''' Simple Constant refering to another contstant
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Gen1()
        Dim bag As New NativeSymbolBag()
        bag.AddConstant(New NativeConstant("C1", "1"))
        bag.AddConstant(New NativeConstant("C2", "1+C1"))
        VerifyConstValue(bag, "C2", String.Format("(1 + {0}.C1)", TransformConstants.NativeConstantsName))
    End Sub

    ''' <summary>
    ''' Simple Enum referring to another enum
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Gen2()
        Dim bag As New NativeSymbolBag()
        Dim e1 As New NativeEnum("e1")
        e1.Values.Add(New NativeEnumValue("v1", "2"))
        e1.Values.Add(New NativeEnumValue("v2", "v1+1"))
        bag.AddDefinedType(e1)
        VerifyEnumValue(bag, e1, "v1", "2")
        VerifyEnumValue(bag, e1, "v2", "(e1.v1 + 1)")
    End Sub

    ''' <summary>
    ''' Cross enumeration reference
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Gen3()
        Dim bag As New NativeSymbolBag()
        Dim e1 As New NativeEnum("e1")
        e1.Values.Add(New NativeEnumValue("v1", "2"))
        e1.Values.Add(New NativeEnumValue("v2", "v1+1"))
        Dim e2 As New NativeEnum("e2")
        e2.Values.Add(New NativeEnumValue("v3", "v2+1"))
        bag.AddDefinedType(e1)
        bag.AddDefinedType(e2)
        VerifyEnumValue(bag, e2, "v3", "(e1.v2 + 1)")
    End Sub

    ''' <summary>
    ''' Cross Constant to Enum reference
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Gen4()
        Dim bag As New NativeSymbolBag()
        bag.AddConstant(New NativeConstant("C1", "42"))
        Dim e1 As New NativeEnum("e1")
        e1.Values.Add(New NativeEnumValue("v1", "C1+2"))
        bag.AddDefinedType(e1)
        VerifyEnumValue(bag, e1, "v1", String.Format("({0}.C1 + 2)", TransformConstants.NativeConstantsName))
    End Sub

    ''' <summary>
    ''' Regression Test
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Gen5()
        Dim bag As New NativeSymbolBag()
        bag.AddConstant(New NativeConstant("A", "A"))
        VerifyConstValue(bag, "A", "Nothing", "System.Object")
    End Sub

    ''' <summary>
    ''' Mutually recursive constants
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Gen6()
        Dim bag As New NativeSymbolBag()
        bag.AddConstant(New NativeConstant("A", "B"))
        bag.AddConstant(New NativeConstant("B", "A"))
        VerifyConstValue(bag, "A", String.Format("{0}.B", TransformConstants.NativeConstantsName))
        VerifyConstValue(bag, "B", String.Format("{0}.A", TransformConstants.NativeConstantsName))
    End Sub

    <TestMethod()> _
    Public Sub Gen7()
        Dim bag As New NativeSymbolBag()
        bag.AddConstant(New NativeConstant("A", "'c'"))
        VerifyConstValue(LanguageType.CSharp, bag, "A", "'c'")
    End Sub

    <TestMethod()> _
    Public Sub Gen8()
        Dim bag As New NativeSymbolBag()
        bag.AddConstant(New NativeConstant("A", "0x5"))
        VerifyConstValue(LanguageType.CSharp, bag, "A", "5")
    End Sub

    <TestMethod()> _
    Public Sub Gen9()
        Dim bag As New NativeSymbolBag()
        bag.AddConstant(New NativeConstant("A", "1.0"))
        VerifyConstValue(LanguageType.CSharp, bag, "A", "1F", "System.Single")
    End Sub

    <TestMethod()> _
      Public Sub Gen10()
        Dim bag As New NativeSymbolBag()
        bag.AddConstant(New NativeConstant("A", "'a'"))
        bag.AddConstant(New NativeConstant("B", "'c'"))
        bag.AddConstant(New NativeConstant("C", "'\n'"))
        VerifyConstValue(LanguageType.CSharp, bag, "A", "'a'", "System.Char")
        VerifyConstValue(LanguageType.CSharp, bag, "B", "'c'", "System.Char")
        VerifyConstValue(LanguageType.CSharp, bag, "C", "'\n'", "System.Char")
    End Sub

    ''' <summary>
    ''' Make sure that unparsable constants are generated as a raw string
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Gen11()
        Dim bag As New NativeSymbolBag()
        bag.AddConstant(New NativeConstant("A", "FALSE;"))
        VerifyConstValue(LanguageType.CSharp, bag, "A", """FALSE;""", "System.String")
    End Sub

    ''' <summary>
    ''' Negative numbers 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Gen12()
        VerifyCSharpExpression("-1", "-1", "System.Int32")
        VerifyCSharpExpression("-1.0F", "-1F", "System.Single")
        VerifyCSharpExpression("-0.1F", "-0.1F", "System.Single")
    End Sub

    ''' <summary>
    ''' Boolean expressions
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Gen13()
        VerifyCSharpExpression("true", "true", "System.Boolean")
        VerifyCSharpExpression("false", "false", "System.Boolean")
    End Sub

    <TestMethod()> _
    Public Sub Gen14()
        Dim bag As New NativeSymbolBag()
        bag.AddConstant(New NativeConstant("A", "L'\10'"))
        VerifyConstValue(LanguageType.CSharp, bag, "A", "'\n'", "System.Char")
    End Sub

    ''' <summary>
    ''' Make sure that an invalid constant expression will still produce a value
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Invalid1()
        Dim bag As New NativeSymbolBag(CreateStandard())
        bag.AddConstant(New NativeConstant("c1", "(S1)5"))
        VerifyConstValue(bag, "c1", """(S1)5""")
    End Sub

    ''' <summary>
    ''' Invalid enum values 
    ''' </summary>
    ''' <remarks></remarks>
    <TestMethod()> _
    Public Sub Invalid2()
        Dim bag As New NativeSymbolBag(CreateStandard())
        Dim e1 As New NativeEnum("e1")
        e1.Values.Add(New NativeEnumValue("v1", "(S1)5"))
        bag.AddDefinedType(e1)
        VerifyEnumValue(bag, e1, "v1", """(S1)5""")
    End Sub

End Class
