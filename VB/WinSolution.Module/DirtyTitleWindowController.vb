Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.SystemModule

Namespace WinSolution.Module
	Public Class DirtyTitleWindowController
		Inherits WindowController
		Public Const DirtyMark As Char = "*"c
		Private windowTemplateController As WindowTemplateController = Nothing
		Protected Overrides Sub OnActivated()
			MyBase.OnActivated()
			AddHandler Window.ViewChanged, AddressOf Window_ViewChanged
			windowTemplateController = Window.GetController(Of WindowTemplateController)()
			AddHandler windowTemplateController.CustomizeWindowCaption, AddressOf windowTemplateController_CustomizeWindowCaption
		End Sub
		Private Sub Window_ViewChanged(ByVal sender As Object, ByVal e As ViewChangedEventArgs)
			If TypeOf Window.View Is ObjectView Then
				AddHandler Window.View.ObjectSpace.ModifiedChanged, AddressOf ObjectSpace_ModifiedChanged
				AddHandler Window.View.ObjectSpace.Reloaded, AddressOf ObjectSpace_Reloaded
			End If
		End Sub
		Private Sub ObjectSpace_Reloaded(ByVal sender As Object, ByVal e As EventArgs)
			UpdateCaption()
		End Sub
		Protected Overridable Sub UpdateCaption()
			If windowTemplateController IsNot Nothing Then
				windowTemplateController.UpdateWindowCaption()
			End If
		End Sub
		Private Sub ObjectSpace_ModifiedChanged(ByVal sender As Object, ByVal e As EventArgs)
			If (CType(sender, IObjectSpace)).IsModified Then
				UpdateCaption()
			End If
		End Sub
		Private Sub windowTemplateController_CustomizeWindowCaption(ByVal sender As Object, ByVal e As CustomizeWindowCaptionEventArgs)
			If Window.View IsNot Nothing Then
				e.WindowCaption.FirstPart = e.WindowCaption.FirstPart.TrimStart(DirtyMark)
				If Window.View.ObjectSpace.IsModified Then
					e.WindowCaption.FirstPart = String.Format("{0} {1}", DirtyMark, e.WindowCaption.FirstPart)
				End If
			End If
		End Sub
		Protected Overrides Sub OnDeactivated()
			RemoveHandler Window.ViewChanged, AddressOf Window_ViewChanged
			If TypeOf Window.View Is ObjectView Then
				RemoveHandler windowTemplateController.CustomizeWindowCaption, AddressOf windowTemplateController_CustomizeWindowCaption
				RemoveHandler Window.View.ObjectSpace.ModifiedChanged, AddressOf ObjectSpace_ModifiedChanged
				RemoveHandler Window.View.ObjectSpace.Reloaded, AddressOf ObjectSpace_Reloaded
				windowTemplateController = Nothing
			End If
			MyBase.OnDeactivated()
		End Sub
	End Class
End Namespace