O aplicativo comprime/minifica as seguintes extensões:

- CSS
- JS
- PNG
- JPG / JPEG
- GIF

ATENÇÃO!!! É necessário permissão de leitura e escrita no arquivo ou na pasta que será executado.

----

Formas de Utilização:

1) BNE.Compress.Console.exe INPUT 
Neste modo a fonte/input é sobrescrita com o resultado, caso ocorra um erro, nenhuma modificação é efetuada.

O INPUT pode ser uma pasta ou um arquivo, caso seja uma pasta sempre será executado de forma recursiva.
Exemplos: 
BNE.Compress.Console.exe "C:\WebSite\" 
BNE.Compress.Console.exe "C:\Test.css"

--

2) BNE.Compress.Console.exe INPUT OUTPUT
Neste modo a fonte não sobre alterações, somente os arquivos com sucesso serão copiados para o output.

O INPUT pode ser uma pasta ou um arquivo, caso seja uma pasta sempre será executado de forma recursiva.
Exemplos: 
BNE.Compress.Console.exe "C:\WebSite\" "C:\WebSiteCompress\" 
BNE.Compress.Console.exe "C:\Test.css" "C:\TestCompress.css"






