/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;

namespace ArithmosModelsTests;

[TestClass]
public class GematriaMethodTests
{
    [TestMethod]
    public void Constructor_WithValidCipher_CreatesInstance()
    {
        // Arrange
        Cipher validCipher = new("A=1,B=2,C=3,D=4,E=5");

        // Act
        GematriaMethod gematriaMethod = new(validCipher);

        // Assert
        Assert.IsNotNull(gematriaMethod);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_WithInvalidCipher1_ThrowsArgumentException()
    {
        // Arrange
        Cipher invalidCipher = new("A=1,B=2,C==3,D=4,E=5"); // Invalid format

        // Act
        GematriaMethod gematriaMethod = new(invalidCipher);

        // Assert
        // Expect an ArgumentException to be thrown
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_WithInvalidCipher2_ThrowsArgumentException()
    {
        // Arrange
        Cipher invalidCipher = new("A=1,B=2,C=3 D=4,E=5"); // Invalid format

        // Act
        GematriaMethod gematriaMethod = new(invalidCipher);

        // Assert
        // Expect an ArgumentException to be thrown
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_WithInvalidCipher3_ThrowsArgumentException()
    {
        // Arrange
        Cipher invalidCipher = new("A=1,B=2,C=3;D=4,E=5"); // Invalid format

        // Act
        GematriaMethod gematriaMethod = new(invalidCipher);

        // Assert
        // Expect an ArgumentException to be thrown
    }

    [TestMethod]
    public void Constructor_WithValidPhrase_HasEqualValueMapperValues()
    {
        // Arrange
        Cipher validCipher = new("A=1,a=1,B=2,b=2,C=3,c=3,D=4,d=4,E=5,e=5,F=6,f=6,G=7,g=7,H=8,h=8,I=9,i=9,J=10,j=10,K=11,k=11,L=12,l=12,M=13,m=13,N=14,n=14,O=15,o=15,P=16,p=16,Q=17,q=17,R=18,r=18,S=19,s=19,T=20,t=20,U=21,u=21,V=22,v=22,W=23,w=23,X=24,x=24,Y=25,y=25,Z=26,z=26");

        // Act
        GematriaMethod gematriaMethod = new(validCipher);

        // Assert
        Assert.AreEqual(gematriaMethod.ValueMapper.Count, 52);
    }

    [TestMethod]
    public void Constructor_WithValidPhrase_HasCorrectValueMapperValues()
    {
        // Arrange
        Cipher validCipher = new("A=1,a=1,B=2,b=2,C=3,c=3,D=4,d=4,E=5,e=5,F=6,f=6,G=7,g=7,H=8,h=8,I=9,i=9,J=10,j=10,K=11,k=11,L=12,l=12,M=13,m=13,N=14,n=14,O=15,o=15,P=16,p=16,Q=17,q=17,R=18,r=18,S=19,s=19,T=20,t=20,U=21,u=21,V=22,v=22,W=23,w=23,X=24,x=24,Y=25,y=25,Z=26,z=26");
        GematriaMethod gematriaMethod = new(validCipher);

        // Act
        string[] pairs = validCipher.Body.Split(',');

        // Assert
        foreach (string pair in pairs)
        {
            string[] values = pair.Split("=");
            Assert.AreEqual(Convert.ToInt32(values[1]), gematriaMethod.ValueMapper[Convert.ToChar(values[0])]);
        }
    }

    [TestMethod]
    public void GetPhraseValue_WithEmptyPhrase_ReturnsZero()
    {
        // Arrange
        Cipher validCipher = new("A=1,B=2,C=3,D=4,E=5");
        GematriaMethod gematriaMethod = new(validCipher)
        {
            Id = 1
        };
        Phrase phrase = new("", [gematriaMethod], 1, 1);

        // Act
        int result = phrase.GetValue(gematriaMethod.Id);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void GetPhraseValue_WithValidPhrase_ReturnsExpectedValue()
    {
        // Arrange
        Cipher validCipher = new("A=1,B=2,C=3,D=4,E=5");
        GematriaMethod gematriaMethod = new(validCipher)
        {
            Id = 1
        };
        Phrase phrase = new("ABC", [gematriaMethod], 10, 3);

        // Act
        int result = phrase.GetValue(gematriaMethod.Id);

        // Assert
        Assert.AreEqual(6, result);
    }

    [TestMethod]
    public void GetPhraseValue_WithValidPhraseWithSpaces_ReturnsExpectedValue()
    {
        // Arrange
        Cipher validCipher = new("A=1,B=2,C=3,D=4,E=5,F=6,G=7,H=8,J=9,T=10,e=4");
        GematriaMethod gematriaMethod = new(validCipher)
        {
            Id = 1
        };
        Phrase phrase = new("This is A test!?!", [gematriaMethod], 10, 3);

        // Act
        int result = phrase.GetValue(gematriaMethod.Id);

        // Assert
        Assert.AreEqual(15, result);
    }

    [TestMethod]
    public void GetPhraseValue_Mixed_RTL_LTR_ReturnsExpectedValue()
    {
        // Arrange
        Cipher validCipher = new("A=1,a=1,B=2,b=2,C=3,c=3,D=4,d=4,E=5,e=5,F=6,f=6,G=7,g=7,H=8,h=8,I=9,i=9,J=10,j=10,K=11,k=11,L=12,l=12,M=13,m=13,N=14,n=14,O=15,o=15,P=16,p=16,Q=17,q=17,R=18,r=18,S=19,s=19,T=20,t=20,U=21,u=21,V=22,v=22,W=23,w=23,X=24,x=24,Y=25,y=25,Z=26,z=26,Α=1,α=1,Β=2,β=2,Γ=3,γ=3,Δ=4,δ=4,Ε=5,ε=5,Ϛ=6,ϛ=6,Ζ=7,ζ=7,Η=8,η=8,Θ=9,θ=9,Ι=10,ι=10,Κ=11,κ=11,Λ=12,λ=12,Μ=13,μ=13,Ν=14,ν=14,Ξ=15,ξ=15,Ο=16,ο=16,Π=17,π=17,Ϙ=18,ϙ=18,Ρ=19,ρ=19,Σ=20,σ=20,ς=20,Τ=21,τ=21,Υ=22,υ=22,Φ=23,φ=23,Χ=24,χ=24,Ψ=25,ψ=25,Ω=26,ω=26,Ϡ=27,ϡ=27,א=1,ב=2,ג=3,ד=4,ה=5,ו=6,ז=7,ח=8,ט=9,י=10,כ=11,ל=12,מ=13,נ=14,ס=15,ע=16,פ=17,צ=18,ק=19,ר=20,ש=21,ת=22,ך=23,ם=24,ן=25,ף=26,ץ=27");
        GematriaMethod gematriaMethod = new(validCipher)
        {
            Id = 1
        };
        Phrase phrase = new("הeε!!!ψφ   Dς פ  ", [gematriaMethod], 10, 3);

        // Act
        int result = phrase.GetValue(gematriaMethod.Id);

        // Assert
        Assert.AreEqual(104, result);
    }

    [TestMethod]
    public void GetTextValue_OnlyFromCharacters_ReturnsExpectedValue()
    {
        // Arrange
        Cipher validCipher = new(Constants.Ciphers.EnglishStandard);
        GematriaMethod gematriaMethod = new(validCipher) { Id = 1 };

        // Act
        int result = gematriaMethod.GetTextValue("The die is cast");

        // Assert
        Assert.AreEqual(644, result);
    }

    [TestMethod]
    public void GetTextValue_WithTotalCharacters_ReturnsExpectedValue()
    {
        // Arrange
        Cipher validCipher = new(Constants.Ciphers.EnglishStandard);
        GematriaMethod gematriaMethod = new(validCipher) { Id = 1, AddsTotalNumberOfCharacters = true };

        // Act
        int result = gematriaMethod.GetTextValue("The die is cast");

        // Assert
        Assert.AreEqual(656, result);
    }

    [TestMethod]
    public void GetTextValue_WithTotalWords_ReturnsExpectedValue()
    {
        // Arrange
        Cipher validCipher = new(Constants.Ciphers.EnglishStandard);
        GematriaMethod gematriaMethod = new(validCipher) { Id = 1, AddsTotalNumberOfWords = true };

        // Act
        int result = gematriaMethod.GetTextValue("The die is cast");

        // Assert
        Assert.AreEqual(648, result);
    }

    [TestMethod]
    public void GetTextValue_WithTotalCharactersAndTotalWords_ReturnsExpectedValue()
    {
        // Arrange
        Cipher validCipher = new(Constants.Ciphers.EnglishStandard);
        GematriaMethod gematriaMethod = new(validCipher) { Id = 1, AddsTotalNumberOfWords = true, AddsTotalNumberOfCharacters = true };

        // Act
        int result = gematriaMethod.GetTextValue("The die is cast");

        // Assert
        Assert.AreEqual(660, result);
    }
}
