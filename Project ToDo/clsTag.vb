Public Class mTag

    Public Property TagDictionary As Dictionary(Of String, String)

    Public Sub New()
        Me.TagDictionary = New Dictionary(Of String, String)()
    End Sub

    Public Sub Add(ByVal key As String, ByVal value As String)
        Me.TagDictionary.Add(key, value)
    End Sub

    Public Sub Update(ByVal key As String, ByVal value As String)
        'Update a Dictionary Item by the key
        For i As Integer = 0 To Me.TagDictionary.Count - 1
            Dim uKey = Me.TagDictionary.ElementAt(i).Key
            If uKey = key Then
                Me.TagDictionary(uKey) = value
                'Me.TagDictionary.Remove(kvp.Key)
                'Me.TagDictionary.Add(key, value)
            End If
        Next
        'For Each kvp As KeyValuePair(Of String, String) In Me.TagDictionary
        '    If kvp.Key = key Then 'If key matches, then update with the new value
        '        'Me.TagDictionary(kvp.Key) = value
        '        Me.TagDictionary.Remove(kvp.Key)
        '        Me.TagDictionary.Add(key, value)
        '        'kvp(kvp.Key) = value
        '        'kvp.Value = value
        '    End If
        'Next
    End Sub

    Public Function [Get](ByVal key As String) As Object
        Return Me.TagDictionary(key)
    End Function

End Class
