﻿' Copyright (c) Microsoft Corporation.  All rights reserved.
'The following code was generated by Microsoft Visual Studio 2005.
'The test owner should check each test for validity.
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System
Imports System.Text
Imports System.Collections.Generic
Imports PInvoke
Imports PInvoke.Transform






'''<summary>
'''This is a test class for PInvoke.Transform.SalAnalyzer and is intended
'''to contain all PInvoke.Transform.SalAnalyzer Unit Tests
'''</summary>
<TestClass()> _
Public Class SalAnalyzerTest


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

    '''<summary>
    '''A test for IsIn()
    '''</summary>
    <TestMethod()> _
    Public Sub IsInTest()
        Dim sal As New NativeSalAttribute( _
            SalEntryType.Pre, SalEntryType.Valid, _
            SalEntryType.Pre, SalEntryType.Deref, SalEntryType.ReadOnly)
        Dim target As SalAnalyzer = New SalAnalyzer(sal)

        Assert.IsTrue(target.IsIn())
    End Sub

    <TestMethod()> _
    Public Sub IsInTest2()
        Dim sal As New NativeSalAttribute( _
            SalEntryType.Pre, SalEntryType.Valid, _
            SalEntryType.Pre, SalEntryType.Deref, SalEntryType.NotReadOnly)
        Dim target As SalAnalyzer = New SalAnalyzer(sal)

        Assert.IsFalse(target.IsIn())
    End Sub

    <TestMethod()> _
    Public Sub ValidIn1()
        Dim sal As New NativeSalAttribute( _
            SalEntryType.Pre, SalEntryType.Valid)
        Dim analyzer As New SalAnalyzer(sal)
        Assert.IsTrue(analyzer.IsValidIn())
        Assert.IsTrue(analyzer.IsValidInOnly())
        Assert.IsFalse(analyzer.IsValidOut())
        Assert.IsFalse(analyzer.IsValidOutOnly)
        Assert.IsFalse(analyzer.IsValidInOut)
    End Sub


    <TestMethod()> _
    Public Sub ValidIn2()
        Dim sal As New NativeSalAttribute( _
            SalEntryType.Pre, SalEntryType.Valid, _
            SalEntryType.Post, SalEntryType.Valid, SalEntryType.Deref, SalEntryType.NotReadOnly)
        Dim analyzer As New SalAnalyzer(sal)
        Assert.IsTrue(analyzer.IsValidIn())
        Assert.IsFalse(analyzer.IsValidInOnly())
        Assert.IsTrue(analyzer.IsValidOut())
        Assert.IsFalse(analyzer.IsValidOutOnly)
        Assert.IsTrue(analyzer.IsValidInOut)
    End Sub

    <TestMethod()> _
    Public Sub ValidOut1()
        Dim sal As New NativeSalAttribute( _
            SalEntryType.Post, SalEntryType.Valid, SalEntryType.Deref, SalEntryType.NotReadOnly)
        Dim analyzer As New SalAnalyzer(sal)
        Assert.IsFalse(analyzer.IsValidIn())
        Assert.IsFalse(analyzer.IsValidInOnly())
        Assert.IsTrue(analyzer.IsValidOut())
        Assert.IsTrue(analyzer.IsValidOutOnly)
        Assert.IsFalse(analyzer.IsValidInOut)
    End Sub

    <TestMethod()> _
    Public Sub ValidOut2()
        Dim sal As New NativeSalAttribute( _
            SalEntryType.Pre, SalEntryType.Valid, _
            SalEntryType.Post, SalEntryType.Valid, SalEntryType.Deref, SalEntryType.NotReadOnly)
        Dim analyzer As New SalAnalyzer(sal)
        Assert.IsTrue(analyzer.IsValidIn())
        Assert.IsFalse(analyzer.IsValidInOnly())
        Assert.IsTrue(analyzer.IsValidOut())
        Assert.IsFalse(analyzer.IsValidOutOnly)
        Assert.IsTrue(analyzer.IsValidInOut)
    End Sub

End Class
