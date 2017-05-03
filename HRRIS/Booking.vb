﻿Option Strict On
Option Explicit On


Imports System.Data.OleDb
Imports System.IO

'Name: booking input and validation form for HRRI systems
'Date: 11 March 2017
'Author: Ly Pham Duy 

'Nore : modifing combo box, checkbox , radio box 
'ACTUALLY, the report can be do by menustrip , do not need to add so much btn 



Public Class Booking
    ' this code is for navigation within form

    Dim lsDataMov As New List(Of Hashtable)
    Dim iCurrentIndex As Integer
    Dim UIModi As New UIController
    Dim bindingsource1 As New BindingSource

    Private Sub Booking_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'HRRISdbDataSet2.booking' table. You can move, or remove it, as needed.
        Me.BookingTableAdapter.Fill(Me.HRRISdbDataSet2.booking)
        'TODO: This line of code loads data into the 'HRRISdbDataSet3.booking' table. You can move, or remove it, as needed.

        'Tooltip part for
        Dim tootipBookg As New ToolTip
        'Input fields part 
        tootipBookg.SetToolTip(txtID, "Input ID to perform program function")
        tootipBookg.SetToolTip(txtDate, "Choose booking date here")
        tootipBookg.SetToolTip(txtType, "Choose room type to show all room that available for that type")
        tootipBookg.SetToolTip(cboCusId, "Choose customer ID from the dropdown box")
        tootipBookg.SetToolTip(cboStays, "Choose customer stays days from the dropdown box")
        tootipBookg.SetToolTip(cboStays, "Choose number of guest from the dropdown box")
        tootipBookg.SetToolTip(txtCheckinDate, "Choose customer checkin date here")
        tootipBookg.SetToolTip(txtPrice, "Booking price will be automatic calculate")
        tootipBookg.SetToolTip(txtCmt, "Input comment here if there are some")
        tootipBookg.SetToolTip(cboRoomID, "RoomID that available and at the choose Type")
        'Button part 
        tootipBookg.SetToolTip(btnAdd, "Click New to input new record")
        tootipBookg.SetToolTip(btnDelete, "Input booking ID to delete record")
        tootipBookg.SetToolTip(btnFind, "Input ID to booking ID to find record")
        tootipBookg.SetToolTip(btnInvoince, "Click here to generate booking invoice")
        tootipBookg.SetToolTip(btnFirst, "Navigation")
        tootipBookg.SetToolTip(btnNext, "Navigation")
        tootipBookg.SetToolTip(btnPrevious, "Navigation")
        tootipBookg.SetToolTip(btnLast, "Navigation")
        tootipBookg.SetToolTip(btnUpdate, "Update booking information")
        starting()

    End Sub
    Private Sub starting()


        'Form part 
        Dim Moving As BookingDataController = New BookingDataController
        lsDataMov = Moving.BookingfindALl()

        'ComboBox Room - traacking data from database And populate() to the box
        'TODO: import ID while showing type"
        cboRoomID.DropDownStyle = ComboBoxStyle.DropDownList
        Dim oController1 As New RoomDataController
        Dim lsData1 = oController1.RoomfindALl()
        For Each Room In lsData1
            cboRoomID.Items.Add(CStr(Room("room_id")))
        Next

        'Comboxbox cusID - traacking data from database and populate() to the box
        'main point: show name , import id
        cboCusId.DropDownStyle = ComboBoxStyle.DropDownList
        Dim ooController As CustomerDataController = New CustomerDataController
        Dim lsData3 = ooController.CusfindALl()
        For Each Customer In lsData3
            cboCusId.Items.Add(CStr(Customer("customer_id")))
        Next

        'allow to gain from ROOm id with room type
        Dim selectedIndex As Integer = cboRoomID.SelectedIndex
        Dim selectedItem As Object = cboRoomID.SelectedItem

        Dim selectedIndex1 As Integer = cboCusId.SelectedIndex
        Dim selectedItem1 As Object = cboCusId.SelectedItem

        'populate() data when open - populateBookingFields()
        Dim htData As Hashtable
        Dim iIndex As Integer
        iIndex = 0
        iCurrentIndex = iIndex
        htData = lsDataMov.Item(iIndex)
        populateBookingFields(lsDataMov.Item(iIndex))

    End Sub
    'Combo box part - using txt inorder to keep track on and modify the code less 
    Private Sub cboRoomID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRoomID.SelectedIndexChanged

        Dim selectedIndex As Integer = cboRoomID.SelectedIndex
        Dim selectedItem As Object = cboRoomID.SelectedItem

    End Sub

    Private Sub cboCusId_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCusId.SelectedIndexChanged
        Dim selectedIndex1 As Integer = cboCusId.SelectedIndex
        Dim selectedItem1 As Object = cboCusId.SelectedItem

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Dim bIsValid = BookValid()

        If bIsValid Then

            Dim bookingData As Hashtable = New Hashtable

            bookingData("customer_id") = cboCusId.Text
            bookingData("booking_date") = txtDate.Text
            bookingData("room_id") = cboRoomID.Text
            bookingData("num_days") = cboStays.Text
            bookingData("num_guests") = cboGuestNum.Text
            bookingData("checkin_date") = txtCheckinDate.Text
            bookingData("total_price") = txtPrice.Text
            bookingData("comments") = txtCmt.Text

            Dim bookingimport As BookingDataController = New BookingDataController
            bookingimport.BookingInsert(bookingData)
            Me.BookingTableAdapter.Fill(Me.HRRISdbDataSet2.booking)
        End If



    End Sub
    'validate private function
    Private Function BookValid() As Boolean

        Dim oValidation As New Validation
        Dim bIsValid As Boolean
        Dim bAllFieldsValid As Boolean = True

        bIsValid = IsNumeric(cboCusId.Text)
        If bIsValid Then
            PicCusID.Visible = False
        Else
            PicCusID.Visible = True
            bAllFieldsValid = False
        End If

        bIsValid = IsNumeric(cboRoomID.Text)
        If bIsValid Then
            PicRoomID.Visible = False
        Else
            PicRoomID.Visible() = True
            bAllFieldsValid = False
        End If

        bIsValid = IsNumeric(cboStays.Text)
        If bIsValid Then
            PicStayingDay.Visible = False

        Else
            PicStayingDay.Visible = True
            bAllFieldsValid = False
        End If

        'Date section
        bIsValid = IsDate(txtDate.Text)
        If bIsValid Then
            PicDate.Visible = False
        Else
            PicDate.Visible = True
            bAllFieldsValid = False
        End If

        bIsValid = IsDate(txtCheckinDate.Text)
        If bIsValid Then
            PicCkinDt.Visible = False
        Else
            PicCkinDt.Visible = True
            bAllFieldsValid = False
        End If

        bIsValid = IsNumeric(cboGuestNum.Text)
        If bIsValid Then
            PicGuestNum.Visible = False
        Else
            PicGuestNum.Visible = True
            bAllFieldsValid = False
        End If

        bIsValid = IsNumeric(txtPrice.Text)
        If bIsValid Then
            PicPrice.Visible = False
        Else
            PicPrice.Visible = True
            bAllFieldsValid = False
        End If

        bIsValid = oValidation.isAlphaNumericVal(txtCmt.Text)
        If bIsValid Then
            PicCmt.Visible = False
        Else
            PicCmt.Visible = True
            bAllFieldsValid = False
        End If

        If bAllFieldsValid Then
            MsgBox("Click OK to import data")
        Else
            MsgBox("Please recheck data where the error pop-up appear")
        End If

        Return bAllFieldsValid

    End Function


    ' CRUD fucntion 
    Private Sub btnFirst_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFirst.Click

        Dim htData As Hashtable
        Dim iIndex As Integer
        iIndex = 0
        iCurrentIndex = iIndex
        htData = lsDataMov.Item(iIndex)
        populateBookingFields(lsDataMov.Item(iIndex))


    End Sub

    Private Sub btnNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNext.Click
        Try
            Dim htData As Hashtable
            Dim iIndex As Integer
            iIndex = iCurrentIndex + 1
            iCurrentIndex = iIndex
            htData = lsDataMov.Item(iIndex)
            populateBookingFields(lsDataMov.Item(iIndex))
        Catch ex As Exception
            MsgBox("end of record")
        End Try

    End Sub

    Private Sub btnPrevious_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrevious.Click
        Try
            Dim htData As Hashtable
            Dim iIndex As Integer
            iIndex = iCurrentIndex - 1
            iCurrentIndex = iIndex
            htData = lsDataMov.Item(iIndex)
            populateBookingFields(lsDataMov.Item(iIndex))
        Catch ex As Exception
            MsgBox("Very first record")
        End Try


    End Sub

    Private Sub btnLast_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLast.Click
        Dim htData As Hashtable
        Dim iIndex As Integer
        iIndex = lsDataMov.Count - 1
        iCurrentIndex = iIndex
        htData = lsDataMov.Item(iIndex)
        populateBookingFields(lsDataMov.Item(iIndex))

    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim oController As BookingDataController = New BookingDataController
        Dim sId = txtID.Text

        Select Case MsgBox("Are you sure to delete this record", MsgBoxStyle.YesNo, "delete")
            Case MsgBoxResult.Yes
                Dim iNumRows = oController.BookingsDelete(sId)
                If iNumRows = 1 Then
                    clearForm()
                    MsgBox("The record was delete")
                End If
            Case MsgBoxResult.No
                MsgBox("The record was not delete")
        End Select

        Me.BookingTableAdapter.Fill(Me.HRRISdbDataSet2.booking)

    End Sub

    Private Sub clearForm()

        txtID.Clear()
        cboRoomID.Items.Clear()
        cboCusId.Items.Clear()
        cboStays.ResetText()
        cboGuestNum.ResetText()
        txtPrice.Clear()
        txtCmt.Clear()
        txtType.ResetText()
        txtRmPrice.Clear()
        txtFirstName.Clear()

    End Sub


    Private Sub btnFind_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFind.Click
        Dim bIsValid As Boolean
        bIsValid = IsNumeric(txtID.Text)

        If bIsValid Then
            Dim oControler As BookingDataController = New BookingDataController
            Dim sId = txtID.Text
            Dim lsData = oControler.BookingsFindById(sId)

            If lsData.Count = 1 Then
                populateBookingFields(lsData.Item(0))

            Else
                MsgBox("booking ID is not valid")
                PicBookID.Visible = True
            End If
        End If

    End Sub

    'Initial function room (checked box) - generate ID to the next 

    'Populate is still missing two combobox - try to figure out how to populate data into a cbo that relating to database dropdown list
    Private Sub populateBookingFields(ByRef bookingData As Hashtable)

        txtID.Text = CStr(CInt(bookingData("booking_id")))
        txtDate.Text = CStr(CDate(bookingData("booking_date")))
        cboRoomID.Text = CStr(CType(bookingData("room_id"), String))
        cboCusId.Text = CStr(CType(bookingData("customer_id"), String))
        cboStays.Text = CStr(CInt(bookingData("num_days")))
        cboGuestNum.Text = CStr(CInt(bookingData("num_guests")))
        txtCheckinDate.Text = CStr(CDate(bookingData("checkin_date")))
        txtPrice.Text = CStr(CInt(bookingData("total_price")))
        txtCmt.Text = CStr(CStr(bookingData("comments")))

    End Sub

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Dim bIsValid = BookValid()


        If bIsValid Then
            Dim oController As BookingDataController = New BookingDataController
            Dim iNumRows = oController.BookingsUpdate(getBookingData)

            If iNumRows = 1 Then
                Debug.Print("The record was updated. check")
            Else
                Debug.Print("The record was not update")

            End If
        End If

        Me.BookingTableAdapter.Fill(Me.HRRISdbDataSet2.booking)

    End Sub

    Private Function getBookingData() As Hashtable
        Dim BookingData As Hashtable = New Hashtable
        BookingData("booking_id") = txtID.Text
        BookingData("booking_date") = txtDate.Text
        BookingData("room_id") = cboRoomID.Text
        BookingData("customer_id") = cboCusId.Text
        BookingData("num_days") = cboStays.Text
        BookingData("num_guests") = cboGuestNum.Text
        BookingData("checkin_date") = txtCheckinDate.Text
        BookingData("total_price") = txtPrice.Text
        BookingData("comments") = txtCmt.Text


        Return BookingData

    End Function

    'Calculating done - add more elseif inorder to provide more right calculation
    Private Sub txtPrice_Leave(sender As Object, e As EventArgs) Handles txtPrice.Leave
        Try
            txtPrice.Text = CStr(CInt(cboStays.Text) * CInt(txtRmPrice.Text))
        Catch ex As Exception
            MsgBox("No room Price or staying date value available")
        End Try


    End Sub

    'Move to invoice form 
    Private Sub btnInvoince_Click(sender As Object, e As EventArgs) Handles btnInvoince.Click

        Dim invoiceform As New Invoice
        invoiceform.bookingIdPass = txtID.Text
        invoiceform.Show()
        Me.Hide()


    End Sub

    'MENU SECTION 
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    'For new record
    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        txtID.Clear()
        txtID.Enabled = False
        btnAdd.Visible = True
        btnDelete.Visible = False
        btnUpdate.Visible = False
        btnFind.Visible = False
        clearForm()

    End Sub
    'To open custoemr form 

    Private Sub CustomerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CustomerToolStripMenuItem.Click
        Dim customer As New Customer
        Me.Hide()
        customer.ShowDialog()
        Me.Close()
    End Sub
    'To open room form 
    Private Sub RoomToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RoomToolStripMenuItem.Click
        Dim bok As New Room
        Me.Hide()
        bok.Show()
        Me.Close()
    End Sub
    'To open report fomr
    Private Sub ReportToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ReportToolStripMenuItem1.Click
        Dim repor1 As New Report
        Me.Hide()
        repor1.Show()
        Me.Close()

    End Sub
    'Edit menu 

    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        txtID.Enabled = True
        btnAdd.Visible = False
        btnDelete.Visible = True
        btnUpdate.Visible = True
        btnFind.Visible = True
        'Starting sub 
        starting()

    End Sub

    'To open break report form
    Private Sub BreakReportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BreakReportToolStripMenuItem.Click
        Dim brkreport1 As New BreakReport
        Me.Hide()
        brkreport1.Show()
        Me.Close()
    End Sub
    'These code is to open the HTML help and about page
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem1.Click
        'This part can be reuse cause it is unchangeable 
        Dim sAbout As String
        sAbout = "About.html "
        Dim sParam As String = """" & Application.StartupPath & "\" & sAbout & """"
        ' the """"" can fix into the access to the file path
        Debug.Print("sParam: " & sParam)

        System.Diagnostics.Process.Start(sParam)
    End Sub
    'These code is to open the HTML help and about page
    Private Sub HelpPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpPageToolStripMenuItem.Click
        'This part can be reuse cause it is unchangeable 
        Dim sHelp As String
        sHelp = "Help.html"
        Dim sParam As String = """" & Application.StartupPath & "\" & sHelp & """"
        ' the """"" can fix into the access to the file path
        Debug.Print("sParam: " & sParam)

        System.Diagnostics.Process.Start(sParam)
    End Sub
    Private Sub InvoiceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InvoiceToolStripMenuItem.Click
        Dim invoicenav As New Invoice
        Me.Hide()
        invoicenav.Show()
        Me.Show()
    End Sub


#Region "enhance -appear customer and room information by performing search in room and customer datacontroller"

    'for the room secttion here. 
    'Due to the reason that this is for record insert. Therefore, only room that have status that "available" = yes 
    'That the room detail can be appear to populate to other fields OR list on to list box on the left hand side 
    Private Sub cboCusId_TextChanged(sender As Object, e As EventArgs) Handles cboCusId.TextChanged

        Dim sCusId = cboCusId.Text

        Dim oController As New CustomerDataController
        Dim lsData = oController.CustomerFind(sCusId)

        If lsData.Count = 1 Then
            populatecus(lsData.Item(0))
        End If

        'Else nothing becasue the software can only work if their is no record in ID fields
        'Reason: for importing new record to database 

    End Sub

    Private Sub cboRoomID_TextChanged(sender As Object, e As EventArgs) Handles cboRoomID.TextChanged

        Dim sRmId = cboRoomID.Text

        Dim oController As New RoomDataController
        Dim lsData = oController.DisplayByRmId(sRmId)

        If lsData.Count = 1 Then
            populateroom(lsData.Item(0))
        End If

        'Else nothing becasue the software can only work if their is no record in ID fields
        'Reason: for importing new record to database 


    End Sub

    Private Sub populatecus(ByRef cusData As Hashtable)

        txtFirstName.Text = CStr(cusData("firstname"))
    End Sub

    Private Sub populateroom(ByRef roomData As Hashtable)

        txtType.Text = CStr(roomData("type"))
        txtRmPrice.Text = CStr(CInt(roomData("price")))

    End Sub


    'Because the type have multiple thinsg
    'Therefore, room record will then be display to the left hand side
    'FOR adding purpose, therefore, txtID have to be nothing.
    Private Sub txtType_TextChanged(sender As Object, e As EventArgs) Handles txtType.TextChanged
        If txtID.Text = Nothing Then

            Dim sType As String = txtType.Text
            Dim lsDataType As List(Of Hashtable)
            Dim oController As New RoomDataController
            lsDataType = oController.DisplayByType(sType)

            LstBox.Visible = True
            Label11.Visible = True

            LstBox.Items.Clear()

            Dim sTDetails As String
            For Each room In lsDataType
                sTDetails = CStr(room("room_id"))
                sTDetails = sTDetails & " | " & CInt(room("room_number"))
                sTDetails = sTDetails & " | " & CStr(room("type"))
                sTDetails = sTDetails & " | " & CInt(room("price"))
                sTDetails = sTDetails & " | " & CInt(room("num_beds"))
                sTDetails = sTDetails & " | " & CStr(room("availability"))
                sTDetails = sTDetails & " | " & CInt(room("floor"))
                sTDetails = sTDetails & " | " & CStr(room("description"))
                LstBox.Items.Add(sTDetails)
            Next



        Else

        End If
    End Sub

    Private Sub populateRoom2(ByRef roomData As Hashtable)

        cboRoomID.Text = CStr(CType(roomData("room_id"), String))
        txtRmPrice.Text = CStr(CInt(roomData("price")))

    End Sub



#End Region


End Class
