Imports System.IO
Imports SendCsvFileFtp.Entities

Module Program

    ' カラム名の設定
    Public ReadOnly csvColumns As New List(Of String)(New String() {
        "test1",
        "test2",
        "test3"
    })

    Sub Main(args As String())

        ' ファイル名の設定
        Dim fileName = "test.csv"

        ' テストデータを作成
        Dim csvInfo = New List(Of CsvEntitiy) From {
            New CsvEntitiy With {
                .test = "test",
                .test2 = 1,
                .test3 = Date.Now(),
                .test4 = "notneed1"
            },
            New CsvEntitiy With {
                .test = "tes1",
                .test2 = 2,
                .test3 = Date.Now().AddDays(1),
                .test4 = "notneed2"
            },
            New CsvEntitiy With {
                .test = "tes2",
                .test2 = 3,
                .test3 = Date.Now().AddDays(2),
                .test4 = "notneed3"
            }
        }

        ' カンマ区切りの文字列のリストを用意
        Dim separatedCommaStrings = New List(Of String)

        ' カラムをカンマ区切りの文字列に変換
        Dim csvColumnStr = String.Join(",", csvColumns)
        separatedCommaStrings.Add(csvColumnStr)

        ' CSV出力に必要な情報をカンマ区切りの文字列に変換
        Dim csvInfoStrings = csvInfo.Select(Function(x) New String($"{ x.test },{ x.test2 },{ x.test3 }")).ToList()

        ' カンマ区切りの文字列に変換して追加
        separatedCommaStrings.AddRange(csvInfoStrings)

        ' カンマ区切りの文字列のリストをバイト情報に変換
        Dim csvInfoBytes = separatedCommaStrings.SelectMany(Function(x) Text.Encoding.UTF8.GetBytes(x + Environment.NewLine)).ToArray()

        ' CSVファイルをFTPで送信
        SendCsvFileFtp(fileName, csvInfoBytes)

    End Sub

    ''' <summary>
    ''' CSVファイルをFTPで送信
    ''' </summary>
    ''' <param name="fileName">ファイル名</param>
    ''' <param name="bytes">バイト情報</param>
    Sub SendCsvFileFtp(fileName As String, bytes As Byte())

        ' ファイル名を変換する
        Dim csvFileName = $"{ fileName.Substring(0, fileName.IndexOf(".")) }str.csv"

        ' サーバ
        Dim ftpServer = "localhost"

        ' Path.Combineは使用できない(Path.Combineはローカル用)
        ' アップロード先のURI
        Dim uploadUri As New Uri($"ftp://{ ftpServer }/{ csvFileName }")

        ' FtpWebRequestの作成
        Dim ftpReq As Net.FtpWebRequest =
            CType(Net.WebRequest.Create(uploadUri), Net.FtpWebRequest)

        ' ログインユーザー名
        Dim loginUserName = "test"

        ' パスワード
        Dim password = "test"

        ' ログインユーザー名とパスワードを設定
        ftpReq.Credentials = New Net.NetworkCredential(loginUserName, password)

        ' アップロードを行う
        ftpReq.Method = Net.WebRequestMethods.Ftp.UploadFile

        ' 要求の完了後に接続を閉じる
        ftpReq.KeepAlive = False

        ' ASCIIモードで転送する
        ftpReq.UseBinary = False

        ' PASVモードを無効にする
        ftpReq.UsePassive = False

        ' 書き込みを行う
        Using reqStream As Stream = ftpReq.GetRequestStream()
            reqStream.Write(bytes, 0, bytes.Length)
        End Using

        'FtpWebResponseを取得
        Using ftpRes As Net.FtpWebResponse =
            CType(ftpReq.GetResponse(), Net.FtpWebResponse)
        End Using

    End Sub

End Module
