﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

#If NETCOREAPP3_1 Then

Namespace Global.Microsoft.VisualBasic.ApplicationServices

    ''' <summary>
    ''' Abstract class that defines the application Startup/Shutdown model for VB
    ''' Windows Applications such as console, winforms, dll, service.
    ''' </summary>
    Public Class ConsoleApplicationBase : Inherits ApplicationBase

        ''' <summary>
        ''' Constructs the application Shutdown/Startup model object
        ''' </summary>
        ''' <remarks>We have to have a parameterless constructor because the platform specific Application
        ''' object derives from this one and it doesn't define a constructor.  The partial class generated by the
        ''' designer defines the constructor in order to configure the application.</remarks>
        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        '''  Returns the command line arguments for the current application.
        ''' </summary>
        ''' <remarks>This function differs from System.Environment.GetCommandLineArgs in that the
        ''' path of the executing file (the 0th entry) is omitted from the returned collection</remarks>
        Public ReadOnly Property CommandLineArgs() As ObjectModel.ReadOnlyCollection(Of String)
            Get
                If _commandLineArgs Is Nothing Then
                    'Get rid of Arg(0) which is the path of the executing program.  Main(args() as string) doesn't report the name of the app and neither will we
                    Dim EnvArgs As String() = Environment.GetCommandLineArgs
                    If EnvArgs.GetLength(0) >= 2 Then '1 element means no args, just the executing program.  >= 2 means executing program + one or more command line arguments
                        Dim NewArgs(EnvArgs.GetLength(0) - 2) As String 'dimming z(0) gives a z() of 1 element.
                        Array.Copy(EnvArgs, 1, NewArgs, 0, EnvArgs.GetLength(0) - 1) 'copy everything but the 0th element (the path of the executing program)
                        _commandLineArgs = New ObjectModel.ReadOnlyCollection(Of String)(NewArgs)
                    Else
                        _commandLineArgs = New ObjectModel.ReadOnlyCollection(Of String)(Array.Empty(Of String))  'provide the empty set
                    End If
                End If
                Return _commandLineArgs
            End Get
        End Property


        ''' <summary>
        ''' Allows derived classes to set what the command line should look like.  WindowsFormsApplicationBase calls this
        ''' for instance because we snag the command line from Main().
        ''' </summary>
        Protected Sub SetInternalCommandLine(value As ObjectModel.ReadOnlyCollection(Of String))
            _commandLineArgs = value
        End Sub

        Private _commandLineArgs As ObjectModel.ReadOnlyCollection(Of String) ' Lazy-initialized and cached collection of command line arguments.
    End Class 'ApplicationBase

End Namespace

#End If
