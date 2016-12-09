Тестовый проект: приложение для регистрации выполняемых на проекте задач. 

Среда разработки: Visual Studio 2013
Технология: ASP.NET MVC
БД: SQL LocalDB (для разработки использовалась JIRA.mdf)

Скрипты для создания таблиц:

CREATE TABLE [dbo].[Person] (
    [Id]         INT         IDENTITY (1, 1) NOT NULL,
    [Name]       NCHAR (100) NOT NULL,
    [Surname]    NCHAR (100) NOT NULL,
    [Middlename] NCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[Task] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [Title]       TEXT     NOT NULL,
    [Description] TEXT     NULL,
    [Start]       DATETIME NULL,
    [End]         DATETIME NULL,
    [State]       INT      NOT NULL,
    [PersonId]    INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([Id])
);

Для подключения иной БД откорректировать параметр connectionstring в файле Web.config.

Основную сложность вызвало изучение и использование ADO.NET (что значительно повлияло на время разработки).