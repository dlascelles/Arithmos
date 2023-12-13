/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;

namespace ArithmosModelsTests;

[TestClass]
public class CipherTests
{
    [TestMethod]
    public void IsValid_ValidInput_ReturnsTrue()
    {
        // Arrange
        string validInput = "a=1,B=33333,p=15,Ω=999999,Ѣ=145";
        Cipher cipher = new(validInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_ValidRTLInput_ReturnsTrue()
    {
        // Arrange
        string validInput = "א=120,ז=90,מ=55";
        Cipher cipher = new(validInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_ValidMixedInput_ReturnsTrue()
    {
        // Arrange
        string validInput = "a=1,b=3,γ=3,א=120,ז=90,מ=55";
        Cipher cipher = new(validInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValid_EmptyInput_ReturnsFalse()
    {
        // Arrange
        string emptyInput = "";
        Cipher cipher = new(emptyInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_InvalidInput_ReturnsFalse()
    {
        // Arrange
        string invalidInput = "invalid-input";
        Cipher cipher = new(invalidInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_InvalidCharacter_ReturnsFalse()
    {
        // Arrange (· is not an allowed character, it should fail.)
        string invalidInput = "a=1,·=33,p=15,Ω=99,Ѣ=145";
        Cipher cipher = new(invalidInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_InvalidValue_ReturnsFalse()
    {
        // Arrange (z is not an allowed value, it should fail.)
        string invalidInput = "a=1,b=33,p=z,Ω=99,Ѣ=145";
        Cipher cipher = new(invalidInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_InvalidSeparatorUsedAsValue_ReturnsFalse()
    {
        // Arrange (z is not an allowed value, it should fail.)
        string invalidInput = "a=1,b==,p=2,Ω=99,Ѣ=145";
        Cipher cipher = new(invalidInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_InvalidCharacterOutOfRange_ReturnsFalse()
    {
        // Arrange (Bb is not a single character, it should fail.)
        string invalidInput = "a=1,Bb=33333,p=15,Ω=999999,Ѣ=145";
        Cipher cipher = new(invalidInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_InvalidValueOutOfRange_ReturnsFalse()
    {
        // Arrange (Ω is 7 characters long. Maximum is 6. This should fail.)
        string invalidInput = "a=1,B=33333,p=15,Ω=9999999,Ѣ=145";
        Cipher cipher = new(invalidInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_InvalidDuplicateCharacter_ReturnsFalse()
    {
        // Arrange
        string invalidInput = "A=1,B=2,C=4,D=15,A=25,d=50,r=45,o=11,t=11,P=13";
        Cipher cipher = new(invalidInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValid_ValidDuplicateValue_ReturnsTrue()
    {
        // Arrange
        string validInput = "A=1,B=2,C=4,D=13,a=25,d=50,r=45,o=11,t=11,P=13";
        Cipher cipher = new(validInput);

        // Act
        bool isValid = cipher.IsValid();

        // Assert
        Assert.IsTrue(isValid);
    }
}
