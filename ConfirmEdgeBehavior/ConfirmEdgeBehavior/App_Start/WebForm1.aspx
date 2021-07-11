<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ConfirmEdgeBehavior.App_Start.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../Scripts/jquery-3.4.1.min.js"></script>
    <script>
        $(function () {

            // 参考サイト
            // https://kamatama41.hatenablog.com/entry/20110615/1308136531

            // ラジオボタンが変更されたら
            $('input[name="lang"]').change(function () {

                // どのラジオボタンが押されたかを取得
                var langIndex = $('[name="lang"]').index(this);

                // 上で取得した番号に応じて、ラジオボタンの状態を変更
                $('input[name="lang1"]:eq(' + langIndex + ')').prop('checked', true);

                // 親要素のCSSを変更
                $(this).parent().css('border', 'double')
                // 下のソースも可
                // $(this).parent('div').css('border', 'double')

            });


        });
    </script>
</head>
<body>
    <form id="form1">
        <div>
            <table border="1">
                <tr>
                    <td rowspan="0">テスト1</td>
                    <td rowspan="1">テスト2</td>
                </tr>
                <tr>
                    <td rowspan="1">テスト3</td>
                    <td rowspan="1">テスト4</td>
                </tr>
            </table>
        </div>

        <div>
            <input type="radio" name="lang" value="ruby" checked="checked" />Ruby
            <input type="radio" name="lang" value="perl" />Perl
            <input type="radio" name="lang" value="test" />Test            
            <input type="radio" name="lang1" value="ruby1" checked="checked" />Ruby1
            <input type="radio" name="lang1" value="perl1" />Perl1
            <input type="radio" name="lang1" value="test1" />Test1
        </div>

    </form>
</body>
</html>
