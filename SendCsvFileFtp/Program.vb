Imports System.IO
Imports SendCsvFileFtp.Entities

Module Program

    ' �J�������̐ݒ�
    Public ReadOnly csvColumns As New List(Of String)(New String() {
        "test1",
        "test2",
        "test3"
    })

    Sub Main(args As String())

        ' �t�@�C�����̐ݒ�
        Dim fileName = "test.csv"

        ' �e�X�g�f�[�^���쐬
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

        ' �J���}��؂�̕�����̃��X�g��p��
        Dim separatedCommaStrings = New List(Of String)

        ' �J�������J���}��؂�̕�����ɕϊ�
        Dim csvColumnStr = String.Join(",", csvColumns)
        separatedCommaStrings.Add(csvColumnStr)

        ' CSV�o�͂ɕK�v�ȏ����J���}��؂�̕�����ɕϊ�
        Dim csvInfoStrings = csvInfo.Select(Function(x) New String($"{ x.test },{ x.test2 },{ x.test3 }")).ToList()

        ' �J���}��؂�̕�����ɕϊ����Ēǉ�
        separatedCommaStrings.AddRange(csvInfoStrings)

        ' �J���}��؂�̕�����̃��X�g���o�C�g���ɕϊ�
        Dim csvInfoBytes = separatedCommaStrings.SelectMany(Function(x) Text.Encoding.UTF8.GetBytes(x + Environment.NewLine)).ToArray()

        ' CSV�t�@�C����FTP�ő��M
        SendCsvFileFtp(fileName, csvInfoBytes)

    End Sub

    ''' <summary>
    ''' CSV�t�@�C����FTP�ő��M
    ''' </summary>
    ''' <param name="fileName">�t�@�C����</param>
    ''' <param name="bytes">�o�C�g���</param>
    Sub SendCsvFileFtp(fileName As String, bytes As Byte())

        ' �t�@�C������ϊ�����
        Dim csvFileName = $"{ fileName.Substring(0, fileName.IndexOf(".")) }str.csv"

        ' �T�[�o
        Dim ftpServer = "localhost"

        ' Path.Combine�͎g�p�ł��Ȃ�(Path.Combine�̓��[�J���p)
        ' �A�b�v���[�h���URI
        Dim uploadUri As New Uri($"ftp://{ ftpServer }/{ csvFileName }")

        ' FtpWebRequest�̍쐬
        Dim ftpReq As Net.FtpWebRequest =
            CType(Net.WebRequest.Create(uploadUri), Net.FtpWebRequest)

        ' ���O�C�����[�U�[��
        Dim loginUserName = "test"

        ' �p�X���[�h
        Dim password = "test"

        ' ���O�C�����[�U�[���ƃp�X���[�h��ݒ�
        ftpReq.Credentials = New Net.NetworkCredential(loginUserName, password)

        ' �A�b�v���[�h���s��
        ftpReq.Method = Net.WebRequestMethods.Ftp.UploadFile

        ' �v���̊�����ɐڑ������
        ftpReq.KeepAlive = False

        ' ASCII���[�h�œ]������
        ftpReq.UseBinary = False

        ' PASV���[�h�𖳌��ɂ���
        ftpReq.UsePassive = False

        ' �������݂��s��
        Using reqStream As Stream = ftpReq.GetRequestStream()
            reqStream.Write(bytes, 0, bytes.Length)
        End Using

        'FtpWebResponse���擾
        Using ftpRes As Net.FtpWebResponse =
            CType(ftpReq.GetResponse(), Net.FtpWebResponse)
        End Using

    End Sub

End Module
