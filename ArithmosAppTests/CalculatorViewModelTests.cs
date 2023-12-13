/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosApp.ViewModels;
using ArithmosModels;

namespace ArithmosAppTests;

[TestClass]
public class CalculatorViewModelTests
{
    [TestMethod]
    public void CalculatorViewModel_CheckInitialValues()
    {
        // Arrange
        CalculatorViewModel calculatorViewModel = new();

        // Act


        // Assert
        Assert.IsTrue(calculatorViewModel.GematriaMethodsViewModels.Count > 0);
        Assert.IsTrue(calculatorViewModel.CalculatorGridSource != null);
        Assert.IsTrue(calculatorViewModel.CalculatorGridSource.Columns[0].Header?.ToString() == "Phrase");
        for (int i = 0; i < calculatorViewModel.GematriaMethodsViewModels.Count; i++)
        {
            Assert.IsTrue(calculatorViewModel.CalculatorGridSource.Columns[i + 1].Header?.ToString() == calculatorViewModel.GematriaMethodsViewModels[i].Name);
        }
        Assert.IsTrue(calculatorViewModel.CalculatorGridSource.Columns[calculatorViewModel.GematriaMethodsViewModels.Count + 1].Header?.ToString() == "Alphabet");
        Assert.IsTrue(calculatorViewModel.CalculatorGridSource.Columns[calculatorViewModel.GematriaMethodsViewModels.Count + 2].Header?.ToString() == "Op. Id");
        Assert.IsTrue(calculatorViewModel.Phrases != null);
        Assert.IsTrue(calculatorViewModel.SaveCurrentPhraseCommand.CanExecute(null) == false);
        Assert.IsTrue(calculatorViewModel.SearchByCurrentPhraseCommand.CanExecute(null) == false);
        Assert.IsTrue(calculatorViewModel.ClearAllResultsCommand.CanExecute(null) == false);
        Assert.IsTrue(calculatorViewModel.CopyCommand.CanExecute(null) == false);
        Assert.IsTrue(calculatorViewModel.CopyCSVCommand.CanExecute(null) == false);
        Assert.IsTrue(calculatorViewModel.ExportGridCommand.CanExecute(null) == false);
        Assert.IsTrue(calculatorViewModel.DeleteSelectedCommand.CanExecute(null) == false);
    }

    [TestMethod]
    public void SaveCurrentPhraseCommand_CanExecuteReturnsTrue()
    {
        // Arrange
        CalculatorViewModel calculatorViewModel = new();

        // Act
        calculatorViewModel.CurrentPhraseContent = "Testing";

        // Assert        
        Assert.IsTrue(calculatorViewModel.SaveCurrentPhraseCommand.CanExecute(null) == true);

    }

    [TestMethod]
    public void SearchByCurrentPhraseAsyncCommand_CanExecuteReturnsTrue()
    {
        // Arrange
        CalculatorViewModel calculatorViewModel = new();

        // Act
        calculatorViewModel.CurrentPhraseContent = "Testing";

        // Assert        
        Assert.IsTrue(calculatorViewModel.SearchByCurrentPhraseCommand.CanExecute(null) == true);

    }

    [TestMethod]
    public void GridCommands_WithoutSelection_CanExecuteChanges()
    {
        // Arrange
        CalculatorViewModel calculatorViewModel = new();

        // Act
        calculatorViewModel.Phrases.Add(new PhraseViewModel(new Phrase("Testing", calculatorViewModel.GetAllGematriaMethods())));

        // Assert        
        Assert.IsTrue(calculatorViewModel.ClearAllResultsCommand.CanExecute(null) == true);
        Assert.IsTrue(calculatorViewModel.CopyCommand.CanExecute(null) == false);
        Assert.IsTrue(calculatorViewModel.CopyCSVCommand.CanExecute(null) == false);
        Assert.IsTrue(calculatorViewModel.ExportGridCommand.CanExecute(null) == true);
        Assert.IsTrue(calculatorViewModel.DeleteSelectedCommand.CanExecute(null) == false);
    }

    [TestMethod]
    public void GridCommands_WithSelection_CanExecuteChanges()
    {
        // Arrange
        CalculatorViewModel calculatorViewModel = new();

        // Act
        calculatorViewModel.Phrases.Add(new PhraseViewModel(new Phrase("Testing", calculatorViewModel.GetAllGematriaMethods())));
        calculatorViewModel.CalculatorGridSource?.RowSelection?.Select(0);
        // Assert        
        Assert.IsTrue(calculatorViewModel.ClearAllResultsCommand.CanExecute(null) == true);
        Assert.IsTrue(calculatorViewModel.CopyCommand.CanExecute(null) == true);
        Assert.IsTrue(calculatorViewModel.CopyCSVCommand.CanExecute(null) == true);
        Assert.IsTrue(calculatorViewModel.ExportGridCommand.CanExecute(null) == true);
        Assert.IsTrue(calculatorViewModel.DeleteSelectedCommand.CanExecute(null) == true);
    }
}