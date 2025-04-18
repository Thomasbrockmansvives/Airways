USE [master]
GO
/****** Object:  Database [FlightBookingDB]    Script Date: 19/04/2025 12:30:53 ******/
CREATE DATABASE [FlightBookingDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FlightBookingDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQL22_VIVES\MSSQL\DATA\FlightBookingDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FlightBookingDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQL22_VIVES\MSSQL\DATA\FlightBookingDB_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [FlightBookingDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FlightBookingDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FlightBookingDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FlightBookingDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FlightBookingDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FlightBookingDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FlightBookingDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [FlightBookingDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FlightBookingDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FlightBookingDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FlightBookingDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FlightBookingDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FlightBookingDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FlightBookingDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FlightBookingDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FlightBookingDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FlightBookingDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FlightBookingDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FlightBookingDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FlightBookingDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FlightBookingDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FlightBookingDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FlightBookingDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FlightBookingDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FlightBookingDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FlightBookingDB] SET  MULTI_USER 
GO
ALTER DATABASE [FlightBookingDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FlightBookingDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FlightBookingDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FlightBookingDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FlightBookingDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FlightBookingDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [FlightBookingDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [FlightBookingDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [FlightBookingDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bookings]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bookings](
	[BookingID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[FlightID] [int] NOT NULL,
	[BookingDate] [date] NOT NULL,
	[TotalPrice] [decimal](8, 2) NOT NULL,
	[MealID] [int] NULL,
	[Class] [nvarchar](20) NOT NULL,
	[SeatNumber] [int] NOT NULL,
 CONSTRAINT [PK_Bookings] PRIMARY KEY CLUSTERED 
(
	[BookingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[CityID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Country] [nvarchar](100) NOT NULL,
	[Dest_Id] [numeric](18, 0) NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[CityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Connections]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Connections](
	[ConnectionID] [int] NOT NULL,
	[DepartureID] [int] NOT NULL,
	[ArrivalID] [int] NOT NULL,
	[Lines] [int] NOT NULL,
	[FlightNumber1] [int] NOT NULL,
	[FlightNumber2] [int] NULL,
	[FlightNumber3] [int] NULL,
 CONSTRAINT [PK_Connections] PRIMARY KEY CLUSTERED 
(
	[ConnectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerPrefs]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerPrefs](
	[PrefID] [int] NOT NULL,
	[ProfileID] [int] NOT NULL,
	[CityID] [int] NOT NULL,
	[VisitCount] [int] NOT NULL,
 CONSTRAINT [PK_CustomerPrefs] PRIMARY KEY CLUSTERED 
(
	[PrefID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerProfiles]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerProfiles](
	[ProfileID] [int] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_CustomerProfiles] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Flights]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flights](
	[FlightNumber] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[UsedSeatsEconomy] [int] NOT NULL,
	[UsedSeatsBusiness] [int] NOT NULL,
	[PriceEconomy] [decimal](8, 2) NOT NULL,
	[PriceBusiness] [decimal](8, 2) NOT NULL,
	[FlightID] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FlightID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lines]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lines](
	[FlightNumber] [int] NOT NULL,
	[DepartureID] [int] NOT NULL,
	[ArrivalID] [int] NOT NULL,
	[DepartureTime] [time](7) NOT NULL,
	[ArrivalTime] [time](7) NOT NULL,
	[TotalSeatsEconomy] [int] NOT NULL,
	[TotalSeatsBusiness] [int] NOT NULL,
 CONSTRAINT [PK_Lines] PRIMARY KEY CLUSTERED 
(
	[FlightNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meals]    Script Date: 19/04/2025 12:30:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meals](
	[MealID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[IsLocalMeal] [bit] NULL,
	[CityID] [int] NULL,
 CONSTRAINT [PK_Meals] PRIMARY KEY CLUSTERED 
(
	[MealID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 19/04/2025 12:30:53 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 19/04/2025 12:30:53 ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 19/04/2025 12:30:53 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 19/04/2025 12:30:53 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 19/04/2025 12:30:53 ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 19/04/2025 12:30:53 ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 19/04/2025 12:30:53 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CustomerPrefs] ADD  CONSTRAINT [DF_CustomerPrefs_VisitCount]  DEFAULT ((0)) FOR [VisitCount]
GO
ALTER TABLE [dbo].[Flights] ADD  CONSTRAINT [DF_Flights_UsedSeatsEconomy]  DEFAULT ((0)) FOR [UsedSeatsEconomy]
GO
ALTER TABLE [dbo].[Flights] ADD  CONSTRAINT [DF_Flights_UsedSeatsBusiness]  DEFAULT ((0)) FOR [UsedSeatsBusiness]
GO
ALTER TABLE [dbo].[Flights] ADD  CONSTRAINT [DF_Flights_PriceEconomy]  DEFAULT ((89.99)) FOR [PriceEconomy]
GO
ALTER TABLE [dbo].[Flights] ADD  CONSTRAINT [DF_Flights_PriceBusiness]  DEFAULT ((189.99)) FOR [PriceBusiness]
GO
ALTER TABLE [dbo].[Lines] ADD  CONSTRAINT [DF_Lines_TotalSeatsEconomy]  DEFAULT ((200)) FOR [TotalSeatsEconomy]
GO
ALTER TABLE [dbo].[Lines] ADD  CONSTRAINT [DF_Lines_TotalSeatsBusiness]  DEFAULT ((50)) FOR [TotalSeatsBusiness]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_CustomerProfiles] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[CustomerProfiles] ([ProfileID])
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_Bookings_CustomerProfiles]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_Flights] FOREIGN KEY([FlightID])
REFERENCES [dbo].[Flights] ([FlightID])
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_Bookings_Flights]
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD  CONSTRAINT [FK_Bookings_Meals] FOREIGN KEY([MealID])
REFERENCES [dbo].[Meals] ([MealID])
GO
ALTER TABLE [dbo].[Bookings] CHECK CONSTRAINT [FK_Bookings_Meals]
GO
ALTER TABLE [dbo].[Connections]  WITH CHECK ADD  CONSTRAINT [FK_Connections_ArrivalCities] FOREIGN KEY([ArrivalID])
REFERENCES [dbo].[Cities] ([CityID])
GO
ALTER TABLE [dbo].[Connections] CHECK CONSTRAINT [FK_Connections_ArrivalCities]
GO
ALTER TABLE [dbo].[Connections]  WITH CHECK ADD  CONSTRAINT [FK_Connections_DepartureCities] FOREIGN KEY([DepartureID])
REFERENCES [dbo].[Cities] ([CityID])
GO
ALTER TABLE [dbo].[Connections] CHECK CONSTRAINT [FK_Connections_DepartureCities]
GO
ALTER TABLE [dbo].[Connections]  WITH CHECK ADD  CONSTRAINT [FK_Connections_Line1] FOREIGN KEY([FlightNumber1])
REFERENCES [dbo].[Lines] ([FlightNumber])
GO
ALTER TABLE [dbo].[Connections] CHECK CONSTRAINT [FK_Connections_Line1]
GO
ALTER TABLE [dbo].[Connections]  WITH CHECK ADD  CONSTRAINT [FK_Connections_Line2] FOREIGN KEY([FlightNumber2])
REFERENCES [dbo].[Lines] ([FlightNumber])
GO
ALTER TABLE [dbo].[Connections] CHECK CONSTRAINT [FK_Connections_Line2]
GO
ALTER TABLE [dbo].[Connections]  WITH CHECK ADD  CONSTRAINT [FK_Connections_Line3] FOREIGN KEY([FlightNumber3])
REFERENCES [dbo].[Lines] ([FlightNumber])
GO
ALTER TABLE [dbo].[Connections] CHECK CONSTRAINT [FK_Connections_Line3]
GO
ALTER TABLE [dbo].[CustomerPrefs]  WITH CHECK ADD  CONSTRAINT [FK_CustomerPrefs_Cities] FOREIGN KEY([CityID])
REFERENCES [dbo].[Cities] ([CityID])
GO
ALTER TABLE [dbo].[CustomerPrefs] CHECK CONSTRAINT [FK_CustomerPrefs_Cities]
GO
ALTER TABLE [dbo].[CustomerPrefs]  WITH CHECK ADD  CONSTRAINT [FK_CustomerPrefs_CustomerProfiles] FOREIGN KEY([ProfileID])
REFERENCES [dbo].[CustomerProfiles] ([ProfileID])
GO
ALTER TABLE [dbo].[CustomerPrefs] CHECK CONSTRAINT [FK_CustomerPrefs_CustomerProfiles]
GO
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_Lines] FOREIGN KEY([FlightNumber])
REFERENCES [dbo].[Lines] ([FlightNumber])
GO
ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_Lines]
GO
ALTER TABLE [dbo].[Lines]  WITH CHECK ADD  CONSTRAINT [FK_Lines_ArrivalCities] FOREIGN KEY([ArrivalID])
REFERENCES [dbo].[Cities] ([CityID])
GO
ALTER TABLE [dbo].[Lines] CHECK CONSTRAINT [FK_Lines_ArrivalCities]
GO
ALTER TABLE [dbo].[Lines]  WITH CHECK ADD  CONSTRAINT [FK_Lines_DepartureCities] FOREIGN KEY([DepartureID])
REFERENCES [dbo].[Cities] ([CityID])
GO
ALTER TABLE [dbo].[Lines] CHECK CONSTRAINT [FK_Lines_DepartureCities]
GO
ALTER TABLE [dbo].[Meals]  WITH CHECK ADD  CONSTRAINT [FK_Meals_Cities] FOREIGN KEY([CityID])
REFERENCES [dbo].[Cities] ([CityID])
GO
ALTER TABLE [dbo].[Meals] CHECK CONSTRAINT [FK_Meals_Cities]
GO
USE [master]
GO
ALTER DATABASE [FlightBookingDB] SET  READ_WRITE 
GO
