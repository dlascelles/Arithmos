/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
namespace ArithmosModels;

public static class Constants
{
    public static class Ciphers
    {
        public const string EnglishStandard = "A=1,a=1,B=2,b=2,C=3,c=3,D=4,d=4,E=5,e=5,F=6,f=6,G=7,g=7,H=8,h=8,I=9,i=9,J=10,j=10,K=20,k=20,L=30,l=30,M=40,m=40,N=50,n=50,O=60,o=60,P=70,p=70,Q=80,q=80,R=90,r=90,S=100,s=100,T=200,t=200,U=300,u=300,V=400,v=400,W=500,w=500,X=600,x=600,Y=700,y=700,Z=800,z=800";

        public const string EnglishOrdinal = "A=1,a=1,B=2,b=2,C=3,c=3,D=4,d=4,E=5,e=5,F=6,f=6,G=7,g=7,H=8,h=8,I=9,i=9,J=10,j=10,K=11,k=11,L=12,l=12,M=13,m=13,N=14,n=14,O=15,o=15,P=16,p=16,Q=17,q=17,R=18,r=18,S=19,s=19,T=20,t=20,U=21,u=21,V=22,v=22,W=23,w=23,X=24,x=24,Y=25,y=25,Z=26,z=26";

        public const string EnglishReduced = "A=1,a=1,B=2,b=2,C=3,c=3,D=4,d=4,E=5,e=5,F=6,f=6,G=7,g=7,H=8,h=8,I=9,i=9,J=1,j=1,K=2,k=2,L=3,l=3,M=4,m=4,N=5,n=5,O=6,o=6,P=7,p=7,Q=8,q=8,R=9,r=9,S=1,s=1,T=2,t=2,U=3,u=3,V=4,v=4,W=5,w=5,X=6,x=6,Y=7,y=7,Z=8,z=8";

        public const string EnglishSumerian = "A=6,a=6,B=12,b=12,C=18,c=18,D=24,d=24,E=30,e=30,F=36,f=36,G=42,g=42,H=48,h=48,I=54,i=54,J=60,j=60,K=66,k=66,L=72,l=72,M=78,m=78,N=84,n=84,O=90,o=90,P=96,p=96,Q=102,q=102,R=108,r=108,S=114,s=114,T=120,t=120,U=126,u=126,V=132,v=132,W=138,w=138,X=144,x=144,Y=150,y=150,Z=156,z=156";

        public const string EnglishPrimes = "A=2,a=2,B=3,b=3,C=5,c=5,D=7,d=7,E=11,e=11,F=13,f=13,G=17,g=17,H=19,h=19,I=23,i=23,J=29,j=29,K=31,k=31,L=37,l=37,M=41,m=41,N=43,n=43,O=47,o=47,P=53,p=53,Q=59,q=59,R=61,r=61,S=67,s=67,T=71,t=71,U=73,u=73,V=79,v=79,W=83,w=83,X=89,x=89,Y=97,y=97,Z=101,z=101";

        public const string EnglishFibonacci = "A=1,a=1,B=1,b=1,C=2,c=2,D=3,d=3,E=5,e=5,F=8,f=8,G=13,g=13,H=21,h=21,I=34,i=34,J=55,j=55,K=89,k=89,L=144,l=144,M=233,m=233,N=377,n=377,O=610,o=610,P=987,p=987,Q=1597,q=1597,R=2584,r=2584,S=4181,s=4181,T=6765,t=6765,U=10946,u=10946,V=17711,v=17711,W=28657,w=28657,X=46368,x=46368,Y=75025,y=75025,Z=121393,z=121393";

        public const string EnglishTetrahedral = "A=1,a=1,B=4,b=4,C=10,c=10,D=20,d=20,E=35,e=35,F=56,f=56,G=84,g=84,H=120,h=120,I=165,i=165,J=220,j=220,K=286,k=286,L=364,l=364,M=455,m=455,N=560,n=560,O=680,o=680,P=816,p=816,Q=969,q=969,R=1140,r=1140,S=1330,s=1330,T=1540,t=1540,U=1771,u=1771,V=2024,v=2024,W=2300,w=2300,X=2600,x=2600,Y=2925,y=2925,Z=3276,z=3276";

        public const string EnglishTriangular = "A=1,a=1,B=3,b=3,C=6,c=6,D=10,d=10,E=15,e=15,F=21,f=21,G=28,g=28,H=36,h=36,I=45,i=45,J=55,j=55,K=66,k=66,L=78,l=78,M=91,m=91,N=105,n=105,O=120,o=120,P=136,p=136,Q=153,q=153,R=171,r=171,S=190,s=190,T=210,t=210,U=231,u=231,V=253,v=253,W=276,w=276,X=300,x=300,Y=325,y=325,Z=351,z=351";


        public const string GreekStandard = "Α=1,α=1,Β=2,β=2,Γ=3,γ=3,Δ=4,δ=4,Ε=5,ε=5,Ϛ=6,ϛ=6,Ζ=7,ζ=7,Η=8,η=8,Θ=9,θ=9,Ι=10,ι=10,Κ=20,κ=20,Λ=30,λ=30,Μ=40,μ=40,Ν=50,ν=50,Ξ=60,ξ=60,Ο=70,ο=70,Π=80,π=80,Ϙ=90,ϙ=90,Ρ=100,ρ=100,Σ=200,σ=200,ς=200,Τ=300,τ=300,Υ=400,υ=400,Φ=500,φ=500,Χ=600,χ=600,Ψ=700,ψ=700,Ω=800,ω=800,Ϡ=900,ϡ=900";

        public const string GreekOrdinal = "Α=1,α=1,Β=2,β=2,Γ=3,γ=3,Δ=4,δ=4,Ε=5,ε=5,Ϛ=6,ϛ=6,Ζ=7,ζ=7,Η=8,η=8,Θ=9,θ=9,Ι=10,ι=10,Κ=11,κ=11,Λ=12,λ=12,Μ=13,μ=13,Ν=14,ν=14,Ξ=15,ξ=15,Ο=16,ο=16,Π=17,π=17,Ϙ=18,ϙ=18,Ρ=19,ρ=19,Σ=20,σ=20,ς=20,Τ=21,τ=21,Υ=22,υ=22,Φ=23,φ=23,Χ=24,χ=24,Ψ=25,ψ=25,Ω=26,ω=26,Ϡ=27,ϡ=27";

        public const string GreekReduced = "Α=1,α=1,Β=2,β=2,Γ=3,γ=3,Δ=4,δ=4,Ε=5,ε=5,Ϛ=6,ϛ=6,Ζ=7,ζ=7,Η=8,η=8,Θ=9,θ=9,Ι=1,ι=1,Κ=2,κ=2,Λ=3,λ=3,Μ=4,μ=4,Ν=5,ν=5,Ξ=6,ξ=6,Ο=7,ο=7,Π=8,π=8,Ϙ=9,ϙ=9,Ρ=1,ρ=1,Σ=2,σ=2,ς=2,Τ=3,τ=3,Υ=4,υ=4,Φ=5,φ=5,Χ=6,χ=6,Ψ=7,ψ=7,Ω=8,ω=8,Ϡ=9,ϡ=9";

        public const string GreekPrimes = "Α=2,α=2,Β=3,β=3,Γ=5,γ=5,Δ=7,δ=7,Ε=11,ε=11,Ϛ=13,ϛ=13,Ζ=17,ζ=17,Η=19,η=19,Θ=23,θ=23,Ι=29,ι=29,Κ=31,κ=31,Λ=37,λ=37,Μ=41,μ=41,Ν=43,ν=43,Ξ=47,ξ=47,Ο=53,ο=53,Π=59,π=59,Ϙ=61,ϙ=61,Ρ=67,ρ=67,Σ=71,σ=71,ς=71,Τ=73,τ=73,Υ=79,υ=79,Φ=83,φ=83,Χ=89,χ=89,Ψ=97,ψ=97,Ω=101,ω=101,Ϡ=103,ϡ=103";

        public const string GreekFibonacci = "Α=1,α=1,Β=1,β=1,Γ=2,γ=2,Δ=3,δ=3,Ε=5,ε=5,Ϛ=8,ϛ=8,Ζ=13,ζ=13,Η=21,η=21,Θ=34,θ=34,Ι=55,ι=55,Κ=89,κ=89,Λ=144,λ=144,Μ=233,μ=233,Ν=377,ν=377,Ξ=610,ξ=610,Ο=987,ο=987,Π=1597,π=1597,Ϙ=2584,ϙ=2584,Ρ=4181,ρ=4181,Σ=6765,σ=6765,ς=6765,Τ=10946,τ=10946,Υ=17711,υ=17711,Φ=28657,φ=28657,Χ=46368,χ=46368,Ψ=75025,ψ=75025,Ω=121393,ω=121393,Ϡ=196418,ϡ=196418";

        public const string GreekTetrahedral = "Α=1,α=1,Β=4,β=4,Γ=10,γ=10,Δ=20,δ=20,Ε=35,ε=35,Ϛ=56,ϛ=56,Ζ=84,ζ=84,Η=120,η=120,Θ=165,θ=165,Ι=220,ι=220,Κ=286,κ=286,Λ=364,λ=364,Μ=455,μ=455,Ν=560,ν=560,Ξ=680,ξ=680,Ο=816,ο=816,Π=969,π=969,Ϙ=1140,ϙ=1140,Ρ=1330,ρ=1330,Σ=1540,σ=1540,ς=1540,Τ=1771,τ=1771,Υ=2024,υ=2024,Φ=2300,φ=2300,Χ=2600,χ=2600,Ψ=2925,ψ=2925,Ω=3276,ω=3276,Ϡ=3654,ϡ=3654";

        public const string GreekTriangular = "Α=1,α=1,Β=3,β=3,Γ=6,γ=6,Δ=10,δ=10,Ε=15,ε=15,Ϛ=21,ϛ=21,Ζ=28,ζ=28,Η=36,η=36,Θ=45,θ=45,Ι=55,ι=55,Κ=66,κ=66,Λ=78,λ=78,Μ=91,μ=91,Ν=105,ν=105,Ξ=120,ξ=120,Ο=136,ο=136,Π=153,π=153,Ϙ=171,ϙ=171,Ρ=190,ρ=190,Σ=210,σ=210,ς=210,Τ=231,τ=231,Υ=253,υ=253,Φ=276,φ=276,Χ=300,χ=300,Ψ=325,ψ=325,Ω=351,ω=351,Ϡ=378,ϡ=378";


        public const string HebrewStandard = "א=1,ב=2,ג=3,ד=4,ה=5,ו=6,ז=7,ח=8,ט=9,י=10,כ=20,ל=30,מ=40,נ=50,ס=60,ע=70,פ=80,צ=90,ק=100,ר=200,ש=300,ת=400,ך=20,ם=40,ן=50,ף=80,ץ=90";

        public const string HebrewOrdinal = "א=1,ב=2,ג=3,ד=4,ה=5,ו=6,ז=7,ח=8,ט=9,י=10,כ=11,ל=12,מ=13,נ=14,ס=15,ע=16,פ=17,צ=18,ק=19,ר=20,ש=21,ת=22,ך=23,ם=24,ן=25,ף=26,ץ=27";

        public const string HebrewReduced = "א=1,ב=2,ג=3,ד=4,ה=5,ו=6,ז=7,ח=8,ט=9,י=1,כ=2,ל=3,מ=4,נ=5,ס=6,ע=7,פ=8,צ=9,ק=1,ר=2,ש=3,ת=4,ך=2,ם=4,ן=5,ף=8,ץ=9";

        public const string HebrewPrimes = "א=2,ב=3,ג=5,ד=7,ה=11,ו=13,ז=17,ח=19,ט=23,י=29,כ=31,ל=37,מ=41,נ=43,ס=47,ע=53,פ=59,צ=61,ק=67,ר=71,ש=73,ת=79,ך=83,ם=89,ן=97,ף=101,ץ=103";

        public const string HebrewFibonacci = "א=1,ב=1,ג=2,ד=3,ה=5,ו=8,ז=13,ח=21,ט=34,י=55,כ=89,ל=144,מ=233,נ=377,ס=610,ע=987,פ=1597,צ=2584,ק=4181,ר=6765,ש=10946,ת=17711,ך=28657,ם=46368,ן=75025,ף=121393,ץ=196418";

        public const string HebrewTetrahedral = "א=1,ב=4,ג=10,ד=20,ה=35,ו=56,ז=84,ח=120,ט=165,י=220,כ=286,ל=364,מ=455,נ=560,ס=680,ע=816,פ=969,צ=1140,ק=1330,ר=1540,ש=1771,ת=2024,ך=2300,ם=2600,ן=2925,ף=3276,ץ=3654";

        public const string HebrewTriangular = "א=1,ב=3,ג=6,ד=10,ה=15,ו=21,ז=28,ח=36,ט=45,י=55,כ=66,ל=78,מ=91,נ=105,ס=120,ע=136,פ=153,צ=171,ק=190,ר=210,ש=231,ת=253,ך=276,ם=300,ן=325,ף=351,ץ=378";

        
        public const string EnglishAlphabetName = "English Alphabet";

        public const string EnglishStandardName = "English Standard";

        public const string EnglishOrdinalName = "English Ordinal";

        public const string EnglishReducedName = "English Reduced";

        public const string EnglishSumerianName = "English Sumerian";

        public const string EnglishPrimesName = "English Primes";

        public const string EnglishFibonacciName = "English Fibonacci";

        public const string EnglishTetrahedralName = "English Tetrahedral";

        public const string EnglishTriangularName = "English Triangular";


        public const string GreekAlphabetName = "Greek Alphabet";

        public const string GreekStandardName = "Greek Standard";

        public const string GreekOrdinalName = "Greek Ordinal";

        public const string GreekReducedName = "Greek Reduced";

        public const string GreekPrimesName = "Greek Primes";

        public const string GreekFibonacciName = "Greek Fibonacci";

        public const string GreekTetrahedralName = "Greek Tetrahedral";

        public const string GreekTriangularName = "Greek Triangular";


        public const string HebrewAlphabetName = "Hebrew Alphabet";

        public const string HebrewStandardName = "Hebrew Standard";

        public const string HebrewOrdinalName = "Hebrew Ordinal";

        public const string HebrewReducedName = "Hebrew Reduced";

        public const string HebrewPrimesName = "Hebrew Primes";

        public const string HebrewFibonacciName = "Hebrew Fibonacci";

        public const string HebrewTetrahedralName = "Hebrew Tetrahedral";

        public const string HebrewTriangularName = "Hebrew Triangular";


        public const string ArabicAlphabetName = "Arabic Alphabet";


        public const string CyrillicAlphabetName = "Cyrillic Alphabet";


        public const string CopticAlphabetName = "Coptic Alphabet";
    }

    public static class Settings
    {
        public const string Theme = "Theme";

        public const string ThemeDark = "Dark";

        public const string ThemeLight = "Light";

        public const string True = "True";

        public const string False = "False";

        public const string ShowColumnOperationId = "ShowColumnOperationId";

        public const string ShowColumnAlphabet = "ShowColumnAlphabet";
    }

    public static class Alphabets
    {
        public static readonly HashSet<char> English = ['A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f', 'G', 'g', 'H', 'h', 'I', 'i', 'J', 'j', 'K', 'k', 'L', 'l', 'M', 'm', 'N', 'n', 'O', 'o', 'P', 'p', 'Q', 'q', 'R', 'r', 'S', 's', 'T', 't', 'U', 'u', 'V', 'v', 'W', 'w', 'X', 'x', 'Y', 'y', 'Z', 'z'];

        public static readonly HashSet<char> Greek = ['Α', 'α', 'Β', 'β', 'Γ', 'γ', 'Δ', 'δ', 'Ε', 'ε', 'Ϛ', 'ϛ', 'Ζ', 'ζ', 'Η', 'η', 'Θ', 'θ', 'Ι', 'ι', 'Κ', 'κ', 'Λ', 'λ', 'Μ', 'μ', 'Ν', 'ν', 'Ξ', 'ξ', 'Ο', 'ο', 'Π', 'π', 'Ϙ', 'ϙ', 'Ρ', 'ρ', 'Σ', 'σ', 'ς', 'Τ', 'τ', 'Υ', 'υ', 'Φ', 'φ', 'Χ', 'χ', 'Ψ', 'ψ', 'Ω', 'ω', 'Ϡ', 'ϡ'];

        public static readonly HashSet<char> Hebrew = ['א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ז', 'ח', 'ט', 'י', 'כ', 'ל', 'מ', 'נ', 'ס', 'ע', 'פ', 'צ', 'ק', 'ר', 'ש', 'ת', 'ך', 'ם', 'ן', 'ף', 'ץ'];

        public static readonly HashSet<char> Arabic = ['ا', 'ب', 'ج', 'د', 'ه', 'و', 'ز', 'ح', 'ط', 'ي', 'ك', 'ل', 'م', 'ن', 'س', 'ع', 'ف', 'ص', 'ق', 'ر', 'ش', 'ت', 'ث', 'خ', 'ذ', 'ض', 'ظ', 'غ', 'ء', 'ة', 'ى'];

        public static readonly HashSet<char> Cyrillic = ['А', 'а', 'Б', 'б', 'В', 'в', 'Г', 'г', 'Д', 'д', 'Е', 'е', 'Є', 'є', 'Ж', 'ж', 'Ꙁ', 'ꙁ', 'З', 'з', 'И', 'и', 'Й', 'й', 'К', 'к', 'Л', 'л', 'М', 'м', 'Н', 'н', 'О', 'о', 'П', 'п', 'Р', 'р', 'С', 'с', 'Т', 'т', 'У', 'у', 'Ф', 'ф', 'Х', 'х', 'Ц', 'ц', 'Ч', 'ч', 'Ш', 'ш', 'Щ', 'щ', 'Ъ', 'ъ', 'Ы', 'ы', 'Ь', 'ь', 'Э', 'э', 'Ю', 'ю', 'Я', 'я', 'Ґ', 'Ђ', 'Ѕ', 'І', 'Ј', 'Љ', 'Њ', 'Ћ', 'Џ'];

        public static readonly HashSet<char> Coptic = ['Ⲁ', 'ⲁ', 'Ⲃ', 'ⲃ', 'Ⲅ', 'ⲅ', 'Ⲇ', 'ⲇ', 'Ⲉ', 'ⲉ', 'Ⲋ', 'ⲋ', 'Ⲍ', 'ⲍ', 'Ⲏ', 'ⲏ', 'Ⲑ', 'ⲑ', 'Ⲓ', 'ⲓ', 'Ⲕ', 'ⲕ', 'Ⲗ', 'ⲗ', 'Ⲙ', 'ⲙ', 'Ⲛ', 'ⲛ', 'Ⲝ', 'ⲝ', 'Ⲟ', 'ⲟ', 'Ⲡ', 'ⲡ', 'Ⲣ', 'ⲣ', 'Ⲥ', 'ⲥ', 'Ⲧ', 'ⲧ', 'Ⲩ', 'ⲩ', 'Ⲫ', 'ⲫ', 'Ⲭ', 'ⲭ', 'Ⲯ', 'ⲯ', 'Ⲱ', 'ⲱ', 'Ⲳ', 'ⲳ', 'Ⲵ', 'ⲵ', 'Ⲷ', 'ⲷ', 'Ⲹ', 'ⲹ', 'Ⲻ', 'ⲻ', 'Ⲽ', 'ⲽ', 'Ⲿ', 'ⲿ', 'Ⳁ', 'ⳁ', 'Ⳃ', 'ⳃ', 'Ⳅ', 'ⳅ', 'Ⳇ', 'ⳇ', 'Ⳉ', 'ⳉ', 'Ⳋ', 'ⳋ', 'Ⳍ', 'ⳍ', 'Ⳏ', 'ⳏ', 'Ⳑ', 'ⳑ', 'Ⳓ', 'ⳓ', 'Ⳕ', 'ⳕ', 'Ⳗ', 'ⳗ', 'Ⳙ', 'ⳙ', 'Ⳛ', 'ⳛ', 'Ⳝ', 'ⳝ', 'Ⳟ', 'ⳟ', 'Ⳡ', 'ⳡ', 'Ⳣ', 'ⳣ', 'ⳤ', '⳥', '⳦', '⳧', '⳨', '⳩', '⳪', 'Ⳬ', 'ⳬ', 'Ⳮ', 'ⳮ', '⳯', '⳱', 'Ⳳ', 'ⳳ', '⳹', '⳺', '⳻', '⳼', '⳽', '⳾', '⳿', 'ϫ', 'ϩ', 'ϥ', 'ϣ', 'ϭ', 'ϯ'];
    }
}
