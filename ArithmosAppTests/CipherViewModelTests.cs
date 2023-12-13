/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosApp.ViewModels;

namespace ArithmosAppTests;

[TestClass]
public class CipherViewModelTests
{
    [TestMethod]
    public void CipherViewModel_CheckInitialValues()
    {
        // Arrange
        CipherViewModel cipherViewModel = new();

        // Act


        // Assert
        Assert.IsTrue(cipherViewModel.GematriaMethodsViewModels.Count > 0);
        Assert.IsTrue(cipherViewModel.ClearCipherPairsCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.AddCipherCommand.CanExecute(null) == true);
        Assert.IsTrue(cipherViewModel.EditCipherCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.MoveUpCipherCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.MoveDownCipherCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.DeleteCipherCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.AddPairCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.AddGroupCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.CancelEditCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.SaveCommand.CanExecute(null) == false);
    }

    [TestMethod]
    public void ClearCipherPairsCommand_CanExecuteReturnsTrue()
    {
        // Arrange
        CipherViewModel cipherViewModel = new();

        // Act        
        cipherViewModel.GematriaCipher.Add(new ValuePairViewModel() { Character = "A", Value = 1 });

        // Assert
        Assert.IsTrue(cipherViewModel.ClearCipherPairsCommand.CanExecute(null) == true);
    }

    [TestMethod]
    public void IsEditModeCommands_CanExecuteChanges()
    {
        // Arrange
        CipherViewModel cipherViewModel = new();

        // Act        
        cipherViewModel.AddCipherCommand.Execute(null);

        // Assert
        Assert.IsTrue(cipherViewModel.IsEditMode);
        Assert.IsTrue(cipherViewModel.AddCipherCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.EditCipherCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.MoveUpCipherCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.MoveDownCipherCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.DeleteCipherCommand.CanExecute(null) == false);
        Assert.IsTrue(cipherViewModel.AddPairCommand.CanExecute(null) == true);
        Assert.IsTrue(cipherViewModel.AddGroupCommand.CanExecute(null) == true);
        Assert.IsTrue(cipherViewModel.CancelEditCommand.CanExecute(null) == true);
        Assert.IsTrue(cipherViewModel.SaveCommand.CanExecute(null) == true);
    }
}
