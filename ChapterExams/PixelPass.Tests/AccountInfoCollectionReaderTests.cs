using System;
using System.IO;
using System.Threading;
using Guts.Client.Core;
using NUnit.Framework;
using PixelPass.Tests.Helpers;

namespace PixelPass.Tests;

[ExerciseTestFixture("dotNet1", "Exams", "PixelPass", @"PixelPass\AccountInfoCollectionReader.cs")]
[Apartment(ApartmentState.STA)]
public class AccountInfoCollectionReaderTests
{
    [MonitoredTest("AccountInfoCollectionReader - Read from file should return collection of AccountInfo"), Order(1)]
    public void _01_ReadFromFile_ShouldReturnCollectionOfAccountInfo()
    {
        // arrange
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string relativePath = @"testdata\pietpienter.txt";
        string filename = Path.Combine(baseDirectory, relativePath);

        // act
         var collection = AccountInfoCollectionReader.Read(filename);

         // assert
        Assert.That(collection, Is.Not.Null);
        Assert.That(collection.Name, Is.EqualTo("Piet Pienter's secrets"));
        Assert.That(collection.AccountInfos, Is.Not.Null);
        Assert.That(collection.AccountInfos.Count, Is.EqualTo(5));
    }

    [MonitoredTest("AccountInfoCollectionReader - Read from a file not starting with Name: should throw ParseException"), Order(2)]
    public void _02_ReadFromFileWithoutNameThrowsParseException()
    {
        // arrange
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string relativePath = @"testdata\pietpienter-noname.txt";
        string filename = Path.Combine(baseDirectory, relativePath);

        // act + assert
        Type parseExceptionType = ParseExceptionHelper.ParseExceptionType;
        Assert.That(parseExceptionType, Is.Not.Null, "Class ParseException is not found");
        Assert.Throws(parseExceptionType, () => AccountInfoCollectionReader.Read(filename), 
             "Read from a file not starting with 'Name:' should throw ParseException. Look for 'pietpienter-noname.txt' in PixelPass.Tests project" );
    }
}