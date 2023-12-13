# Arithmos - Gematria Calculator and Scanner

A desktop application that can be used to calculate and store the gematria (numerical values) of words. 

You can scan books in the form of text files to look for specific values or even import an entire book by extracting all words and phrases along with their corresponding gematria values.

The application also allows you to create your own custom ciphers / gematria methods.

It supports the *English*, *Greek*, *Hebrew*, *Arabic*, *Cyrillic* and *Coptic* alphabets and can handle diacritics without issues.


## Running the application

There is only a single executable file ("Arithmos.exe") that you need to download and there is no installation process.

The first time you run the application, it will create a database file named "ArithmosDatabase.sqlite" at the same location with the executable. Also any CSV files you generate with the application will be placed at the same location as well.

**Download the latest stable release from here: [Arithmos Latest Release](https://github.com/dlascelles/Arithmos/releases)**


## Version 3 Notes

Arithmos 3 is almost completely rewritten from scratch, with a different database structure and a new UI framework. Therefore it is not compatible with earlier versions. The main reason for the rewrite was to allow the users to create their own custom gematria methods / ciphers, and the previous version was too 'rigid' in its design to allow for that. You can run the two versions side by side and they won't conflict with each other, as they are basically two different applications.


## Arithmos Screenshots and Features

### Gematria Calculator

![Calculator](https://github.com/dlascelles/Arithmos/assets/52032313/f2fb9e17-7a5b-430f-867f-beb02c24cbd0)

From the Calculator screen you can type anything and get the corresponding gematria values as you type. The ciphers can be completely customized. By clicking the "Search database" button you can look for phrases and words stored in your database that have the same values. You have the option to select only those gematria methods that you are interested to search for.


### Gematria Scanner

![Scanner](https://github.com/dlascelles/Arithmos/assets/52032313/b6267efd-ea57-4df6-a9ac-0e98c047a17b)

You can use the Scanner to scan a text file, or text you insert into the text-box. There are many options available to fine-tune the scanner according to your needs. If you find any results that interest you, you can batch-save them grouped under an 'Operation'. You can later use the Explorer to load an entire operation or even delete it if you don't want it anymore.


### Database Explorer

![Explorer](https://github.com/dlascelles/Arithmos/assets/52032313/d5841950-64a2-4422-bf7d-3b28856811f3)

The Explorer can be used to search for phrases stored in your database, either by value, text or operation. You can copy the results to the clipboard or export them to a CSV file for use in another application like LibreOffice Calc or Excel.


### Custom Gematria Methods

![Ciphers](https://github.com/dlascelles/Arithmos/assets/52032313/cb426608-6c76-486a-a485-182d6c601d07)

You can use the Cipher Editor to add, edit or delete gematria methods. Any letter can be mapped to any numeric value. It also allows you to add the total number of words or characters to the final gematria value if you want to. There are also several built-in templates with alphabets and value correspondences that you can use to save time.


If you find any issues please let me know and I'll do my best to address them. I'm working on it in my spare time so development will likely be slow. I hope you enjoy the application as much as I enjoyed building it!


## License

MIT License

Copyright (c) 2018 - 2024 Daniel Lascelles

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.