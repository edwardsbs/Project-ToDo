Public Module modClasses
    Public Class ToDoSystems
        Property ID As Integer
        Property System As String
        Property Creator As String
        Property Owner As String
        Property Developer As String
        Property Description As String
        Property Applyby As String
        Property iIndex As Integer
        Property Phase As String
        Property Expanded As Boolean
        Property Scope As ProjectScope
        Property Dept As String
        'Property Users As String
        'Property PIC As String
        Property Status As String
        Property ToDo As List(Of ToDoItem)


    End Class
    Public Class ToDoItem
        Property Project As String
        Property ToDo As String
        Property Type As String
        Property Path As String
        Property Scope As ProjectScope

        Property SubItems As List(Of ToDoSubItem)
        Property Schedule As ToDoSchedule
        Property Note As Notes

    End Class
    Public Class ToDoSchedule
        Property Phase As String
        Property Status As String
        Property PlanStart As Date
        Property PlanEnd As Date
        Property ActualStart As Date
        Property ActualEnd As Date
        Property EstTime As Long
        Property EstTimeUoM As String
        Property ActualTime As Long
        Property ActualTimeUoM As String
        Property CompPercent As Long
        Property PublishDate As Date
        Property MtgReviewComplete As Boolean

    End Class
    Public Class ToDoSubItem
        Property Parent As String
        Property Schedule As ToDoSchedule
        Property Note As Notes
    End Class

    Public Class Notes
        Property ID As Integer
        Property Note As String
        Property ImageID As List(Of Images)

    End Class

    Public Class Images
        Property ImageID As Integer
        Property Image As Image
        Property ImageNote As String

    End Class
    Public Class Journal
        Property ID As Integer
        Property WorkDescription As String
        Property WorkDatePlan As Date
        Property WorkHrsPlan As Long
        Property WorkDateActual As Date
        Property WorkHrsActual As Long
        Property Notes As String

    End Class

    Public Class ProjectScope
        Property ScopeID As Integer
        Property Scope As String
        Property Objective As String
        Property Benefits As String
        Property Accomplishments As String
        Property Notes As String
        Property IssuesRisks As String
        Property Changes As String
        'Property Scope As String
    End Class

End Module
