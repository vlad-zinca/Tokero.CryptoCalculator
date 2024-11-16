-- Create Database
CREATE DATABASE Tokero;

USE Tokero;

-- Create Currencies Table
CREATE TABLE Currencies (
	Id INT PRIMARY KEY IDENTITY (1,1),
    CurrencyId INT,
    Name NVARCHAR(1000) NOT NULL,
    Symbol NVARCHAR(1000) NOT NULL,
    Slug NVARCHAR(1000) NOT NULL,
    NumMarketPairs INT NOT NULL,
    DateAdded DATETIME NOT NULL,
    Rank INT NOT NULL,
    LastUpdated DATETIME NOT NULL,
	Price DECIMAL(18,4) NOT NULL
);