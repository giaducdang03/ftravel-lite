USE [FTravel_Lite]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 19/10/2024 8:17:40 SA ******/
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
/****** Object:  Table [dbo].[City]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[UnsignName] [nvarchar](100) NULL,
	[Code] [int] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__City__3214EC07C67B929F] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Type] [nvarchar](50) NULL,
	[IsRead] [bit] NOT NULL,
	[Title] [nvarchar](100) NULL,
	[Message] [nvarchar](500) NULL,
	[EntityId] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Notifica__3214EC07FB2BB517] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[TotalPrice] [int] NULL,
	[PaymentDate] [datetime2](7) NULL,
	[PaymentStatus] [nvarchar](50) NULL,
	[UserId] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Order__3214EC077B4E0DAE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TicketId] [int] NULL,
	[OrderId] [int] NULL,
	[Type] [nvarchar](10) NULL,
	[ServiceName] [nvarchar](150) NULL,
	[UnitPrice] [int] NULL,
	[Quantity] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__OrderDet__3214EC0722A0C253] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Otp]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Otp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[OtpCode] [nvarchar](6) NOT NULL,
	[ExpiryTime] [datetime2](7) NOT NULL,
	[IsUsed] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Otp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Route]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Route](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[StartPoint] [int] NULL,
	[EndPoint] [int] NULL,
	[Status] [nvarchar](50) NULL,
	[UnsignName] [nvarchar](100) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Route__3214EC076D3E3823] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RouteStation]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RouteStation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RouteId] [int] NULL,
	[StationId] [int] NULL,
	[StationIndex] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__RouteSta__3214EC07003F60C2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RouteId] [int] NULL,
	[StationId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DefaultPrice] [int] NULL,
	[ImgUrl] [nvarchar](max) NULL,
	[ShortDescription] [nvarchar](200) NULL,
	[FullDescription] [nvarchar](max) NULL,
	[UnsignName] [nvarchar](100) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Service__3214EC0758760C60] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceTicket]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceTicket](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NULL,
	[TicketId] [int] NULL,
	[Price] [int] NULL,
	[Quantity] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__ServiceT__3214EC07E7FD96DF] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Station]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Station](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](250) NULL,
	[Status] [nvarchar](50) NULL,
	[CityId] [int] NULL,
	[UnsignName] [nvarchar](100) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Station__3214EC076CCE1564] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticket]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticket](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TripId] [int] NULL,
	[TicketTypeId] [int] NULL,
	[SeatCode] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Ticket__3214EC0720D026DC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TicketType]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TicketType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[RouteId] [int] NULL,
	[Price] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__TicketTy__3214EC07BF501A49] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WalletId] [int] NOT NULL,
	[TransactionType] [nvarchar](50) NOT NULL,
	[Amount] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[TransactionDate] [datetime2](7) NULL,
	[Status] [nvarchar](50) NOT NULL,
	[OrderId] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
	[TrasactionCode] [int] NOT NULL,
 CONSTRAINT [PK__Transact__3214EC074E82A4E2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trip]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trip](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[RouteId] [int] NULL,
	[OpenTicketDate] [datetime2](7) NULL,
	[EstimatedStartDate] [datetime2](7) NULL,
	[EstimatedEndDate] [datetime2](7) NULL,
	[ActualStartDate] [datetime2](7) NULL,
	[ActualEndDate] [datetime2](7) NULL,
	[Status] [nvarchar](50) NULL,
	[IsTemplate] [bit] NULL,
	[DriverId] [int] NULL,
	[UnsignName] [nvarchar](100) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Trip__3214EC07FAD9A29D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TripService]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TripService](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TripId] [int] NULL,
	[ServiceId] [int] NULL,
	[ServicePrice] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__TripServ__3214EC07FBCF2CC6] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TripTicketType]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TripTicketType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TripId] [int] NULL,
	[TicketTypeId] [int] NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__TripTick__3214EC07B3BF44EC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[ConfirmEmail] [bit] NULL,
	[PasswordHash] [nvarchar](500) NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[DOB] [datetime2](7) NULL,
	[PhoneNumber] [varchar](10) NULL,
	[Address] [nvarchar](255) NULL,
	[Gender] [int] NULL,
	[Status] [nvarchar](50) NULL,
	[AvatarUrl] [nvarchar](max) NULL,
	[GoogleId] [nvarchar](64) NULL,
	[FCMToken] [nvarchar](255) NULL,
	[Role] [nvarchar](5) NULL,
	[UnsignFullName] [nvarchar](100) NULL,
	[PIN] [varchar](6) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__User__3214EC07C0410F19] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 19/10/2024 8:17:40 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[AccountBalance] [int] NULL,
	[Status] [nvarchar](50) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__Wallet__3214EC07DC2620DF] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241017153343_InitDb', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241017155628_UpdateTableUser', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241018125114_UpdateTableTrans', N'8.0.0')
GO
SET IDENTITY_INSERT [dbo].[City] ON 

INSERT [dbo].[City] ([Id], [Name], [UnsignName], [Code], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, N'Hồ Chí Minh', N'Ho Chi Minh', 79, CAST(N'2024-10-18T21:54:47.8800000' AS DateTime2), NULL, 0)
INSERT [dbo].[City] ([Id], [Name], [UnsignName], [Code], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, N'Long An', N'Long An', 80, CAST(N'2024-10-18T21:56:53.9200000' AS DateTime2), NULL, 0)
INSERT [dbo].[City] ([Id], [Name], [UnsignName], [Code], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (3, N'Vũng Tàu', N'Vung Tau', 77, CAST(N'2024-10-18T21:57:02.4900000' AS DateTime2), NULL, 0)
INSERT [dbo].[City] ([Id], [Name], [UnsignName], [Code], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (4, N'Cần Thơ', N'Can Tho', 92, CAST(N'2024-10-18T21:57:10.7966667' AS DateTime2), NULL, 0)
INSERT [dbo].[City] ([Id], [Name], [UnsignName], [Code], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (5, N'Khánh Hòa', N'Khanh Hoa', 56, CAST(N'2024-10-18T21:57:21.2233333' AS DateTime2), NULL, 0)
INSERT [dbo].[City] ([Id], [Name], [UnsignName], [Code], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (6, N'Đồng Nai', N'Dong Nai', 75, CAST(N'2024-10-18T23:05:53.0300000' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[City] OFF
GO
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([Id], [Code], [TotalPrice], [PaymentDate], [PaymentStatus], [UserId], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, N'20241019003600', 331, CAST(N'2024-10-19T00:36:00.8655611' AS DateTime2), N'SUCCESS', 4, CAST(N'2024-10-19T00:36:00.5156004' AS DateTime2), CAST(N'2024-10-19T00:36:00.8861955' AS DateTime2), 0)
INSERT [dbo].[Order] ([Id], [Code], [TotalPrice], [PaymentDate], [PaymentStatus], [UserId], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, N'20241019003818', 331, CAST(N'2024-10-19T00:38:43.1385492' AS DateTime2), N'FAILED', 4, CAST(N'2024-10-19T00:38:27.8231480' AS DateTime2), CAST(N'2024-10-19T00:38:43.1534855' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetail] ON 

INSERT [dbo].[OrderDetail] ([Id], [TicketId], [OrderId], [Type], [ServiceName], [UnitPrice], [Quantity], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, 29, 1, N'Ticket', NULL, 331, 1, CAST(N'2024-10-19T00:36:00.6400000' AS DateTime2), NULL, 0)
INSERT [dbo].[OrderDetail] ([Id], [TicketId], [OrderId], [Type], [ServiceName], [UnitPrice], [Quantity], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, 30, 2, N'Ticket', NULL, 331, 1, CAST(N'2024-10-19T00:38:27.8800000' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[OrderDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[Otp] ON 

INSERT [dbo].[Otp] ([Id], [Email], [OtpCode], [ExpiryTime], [IsUsed], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, N'giaducdang@gmail.com', N'148390', CAST(N'2024-10-17T23:56:11.2471503' AS DateTime2), 1, CAST(N'2024-10-17T23:51:11.2474212' AS DateTime2), CAST(N'2024-10-17T23:51:47.4161558' AS DateTime2), 0)
INSERT [dbo].[Otp] ([Id], [Email], [OtpCode], [ExpiryTime], [IsUsed], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, N'giaduc0123@gmail.com', N'372415', CAST(N'2024-10-18T00:00:29.9836926' AS DateTime2), 0, CAST(N'2024-10-17T23:55:29.9839630' AS DateTime2), NULL, 0)
INSERT [dbo].[Otp] ([Id], [Email], [OtpCode], [ExpiryTime], [IsUsed], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (3, N'giaducdang@gmail.com', N'580189', CAST(N'2024-10-18T19:39:20.8903297' AS DateTime2), 0, CAST(N'2024-10-18T19:34:20.8910739' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[Otp] OFF
GO
SET IDENTITY_INSERT [dbo].[Route] ON 

INSERT [dbo].[Route] ([Id], [Name], [StartPoint], [EndPoint], [Status], [UnsignName], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, N'Hồ Chí Minh - Vũng Tàu', 1, 3, N'ACTIVE', N'Ho Chi Minh   Vung Tau', CAST(N'2024-10-18T22:14:47.2522076' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[Route] OFF
GO
SET IDENTITY_INSERT [dbo].[RouteStation] ON 

INSERT [dbo].[RouteStation] ([Id], [RouteId], [StationId], [StationIndex], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, 1, 1, 1, CAST(N'2024-10-18T23:48:43.5193406' AS DateTime2), NULL, 0)
INSERT [dbo].[RouteStation] ([Id], [RouteId], [StationId], [StationIndex], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, 1, 2, 2, CAST(N'2024-10-18T23:54:53.5664001' AS DateTime2), NULL, 0)
INSERT [dbo].[RouteStation] ([Id], [RouteId], [StationId], [StationIndex], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (3, 1, 3, 3, CAST(N'2024-10-18T23:55:05.8563006' AS DateTime2), NULL, 0)
INSERT [dbo].[RouteStation] ([Id], [RouteId], [StationId], [StationIndex], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (4, 1, 4, 4, CAST(N'2024-10-18T23:55:18.8180005' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[RouteStation] OFF
GO
SET IDENTITY_INSERT [dbo].[Service] ON 

INSERT [dbo].[Service] ([Id], [RouteId], [StationId], [Name], [DefaultPrice], [ImgUrl], [ShortDescription], [FullDescription], [UnsignName], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, 1, 1, N'Nước suối', 8, N'https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2Faquafina-500ml.jpg?alt=media&token=2c1f05cc-605c-4d9c-9b7c-dde43e0f0761', N'Nước suối', N'Nước suối', N'Nuoc suoi', CAST(N'2024-10-19T00:01:19.7520336' AS DateTime2), NULL, 0)
INSERT [dbo].[Service] ([Id], [RouteId], [StationId], [Name], [DefaultPrice], [ImgUrl], [ShortDescription], [FullDescription], [UnsignName], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, 1, 3, N'Bánh mì', 15, N'https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2Fbanhmi.jpg?alt=media&token=33a159d3-9f4e-42ec-9df9-446cdb7586d3', N'Bánh mì', N'Bánh mì', N'Banh mi', CAST(N'2024-10-19T00:02:08.1570326' AS DateTime2), NULL, 0)
INSERT [dbo].[Service] ([Id], [RouteId], [StationId], [Name], [DefaultPrice], [ImgUrl], [ShortDescription], [FullDescription], [UnsignName], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (3, 1, 3, N'Hủ tiếu', 30, N'https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2Fhu-tieu.jpg?alt=media&token=b0227226-d283-4fb7-b7a8-ff8ea37ee9d4', N'Hủ tiếu', N'Hủ tiếu', N'Hu tieu', CAST(N'2024-10-19T00:03:03.7243046' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[Service] OFF
GO
SET IDENTITY_INSERT [dbo].[ServiceTicket] ON 

INSERT [dbo].[ServiceTicket] ([Id], [ServiceId], [TicketId], [Price], [Quantity], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, 1, 29, 8, 2, CAST(N'2024-10-19T00:36:00.2194600' AS DateTime2), NULL, 0)
INSERT [dbo].[ServiceTicket] ([Id], [ServiceId], [TicketId], [Price], [Quantity], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, 2, 29, 15, 1, CAST(N'2024-10-19T00:36:00.2197143' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[ServiceTicket] OFF
GO
SET IDENTITY_INSERT [dbo].[Station] ON 

INSERT [dbo].[Station] ([Id], [Name], [Address], [Status], [CityId], [UnsignName], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, N'Bến xe Bến Thành', N'Quận 1', N'ACTIVE', 1, N'Ben xe Ben Thanh', CAST(N'2024-10-18T22:55:32.4293720' AS DateTime2), NULL, 0)
INSERT [dbo].[Station] ([Id], [Name], [Address], [Status], [CityId], [UnsignName], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, N'Bến xe An Sương', N'Quận 12', N'ACTIVE', 1, N'Ben xe An Suong', CAST(N'2024-10-18T23:03:58.4757947' AS DateTime2), NULL, 0)
INSERT [dbo].[Station] ([Id], [Name], [Address], [Status], [CityId], [UnsignName], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (3, N'Bò sữa Long Thành', N'Long Thành', N'ACTIVE', 6, N'Bo sua Long Thanh', CAST(N'2024-10-18T23:06:07.4019723' AS DateTime2), NULL, 0)
INSERT [dbo].[Station] ([Id], [Name], [Address], [Status], [CityId], [UnsignName], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (4, N'Bến xe Vũng Tàu', N'Tp. Vũng Tàu', N'ACTIVE', 3, N'Ben xe Vung Tau', CAST(N'2024-10-18T23:06:57.0042245' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[Station] OFF
GO
SET IDENTITY_INSERT [dbo].[Ticket] ON 

INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, 1, 1, N'A1', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, 1, 1, N'A2', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (3, 1, 1, N'A3', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (4, 1, 1, N'A4', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (5, 1, 1, N'B1', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (6, 1, 1, N'B2', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (7, 1, 1, N'B3', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (8, 1, 1, N'B4', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (9, 1, 1, N'C1', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (10, 1, 1, N'C2', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (11, 1, 1, N'C3', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (12, 1, 1, N'C4', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (13, 1, 1, N'D1', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (14, 1, 1, N'D2', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (15, 1, 1, N'D3', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (16, 1, 1, N'D4', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (17, 1, 1, N'E1', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (18, 1, 1, N'E2', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (19, 1, 1, N'E3', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (20, 1, 1, N'E4', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (21, 1, 1, N'F1', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (22, 1, 1, N'F2', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (23, 1, 1, N'F3', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (24, 1, 1, N'F4', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (25, 1, 1, N'G1', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (26, 1, 1, N'G2', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (27, 1, 1, N'G3', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (28, 1, 1, N'G4', N'AVAILABLE', CAST(N'2024-10-19T00:07:46.6033333' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (29, 2, 1, N'A1', N'SOLD', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), CAST(N'2024-10-19T00:36:00.8986024' AS DateTime2), 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (30, 2, 1, N'A2', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (31, 2, 1, N'A3', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (32, 2, 1, N'A4', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (33, 2, 1, N'B1', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (34, 2, 1, N'B2', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (35, 2, 1, N'B3', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (36, 2, 1, N'B4', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (37, 2, 1, N'C1', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (38, 2, 1, N'C2', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (39, 2, 1, N'C3', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (40, 2, 1, N'C4', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (41, 2, 1, N'D1', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (42, 2, 1, N'D2', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (43, 2, 1, N'D3', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (44, 2, 1, N'D4', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (45, 2, 1, N'E1', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (46, 2, 1, N'E2', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (47, 2, 1, N'E3', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (48, 2, 1, N'E4', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (49, 2, 1, N'F1', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (50, 2, 1, N'F2', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (51, 2, 1, N'F3', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (52, 2, 1, N'F4', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (53, 2, 1, N'G1', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (54, 2, 1, N'G2', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (55, 2, 1, N'G3', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
INSERT [dbo].[Ticket] ([Id], [TripId], [TicketTypeId], [SeatCode], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (56, 2, 1, N'G4', N'AVAILABLE', CAST(N'2024-10-19T00:15:58.9800000' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[Ticket] OFF
GO
SET IDENTITY_INSERT [dbo].[TicketType] ON 

INSERT [dbo].[TicketType] ([Id], [Name], [RouteId], [Price], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, N'VIP', 1, 300, CAST(N'2024-10-18T00:00:00.0000000' AS DateTime2), NULL, 0)
INSERT [dbo].[TicketType] ([Id], [Name], [RouteId], [Price], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, N'Normal', 1, 200, CAST(N'2024-10-18T00:00:00.0000000' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[TicketType] OFF
GO
SET IDENTITY_INSERT [dbo].[Transaction] ON 

INSERT [dbo].[Transaction] ([Id], [WalletId], [TransactionType], [Amount], [Description], [TransactionDate], [Status], [OrderId], [CreateDate], [UpdateDate], [IsDeleted], [TrasactionCode]) VALUES (3, 2, N'IN', 500, N'Nạp FToken vào ví Tran Tina từ VNPAY', CAST(N'2024-10-18T19:52:53.0000000' AS DateTime2), N'SUCCESS', NULL, CAST(N'2024-10-18T19:52:12.0861759' AS DateTime2), CAST(N'2024-10-18T19:52:39.3306340' AS DateTime2), 0, 471026)
INSERT [dbo].[Transaction] ([Id], [WalletId], [TransactionType], [Amount], [Description], [TransactionDate], [Status], [OrderId], [CreateDate], [UpdateDate], [IsDeleted], [TrasactionCode]) VALUES (4, 2, N'OUT', 331, N'Thanh toán cho đơn hàng 20241019003600', CAST(N'2024-10-19T00:36:00.8655611' AS DateTime2), N'SUCCESS', 1, CAST(N'2024-10-19T00:36:00.6863525' AS DateTime2), CAST(N'2024-10-19T00:36:00.8658786' AS DateTime2), 0, 0)
INSERT [dbo].[Transaction] ([Id], [WalletId], [TransactionType], [Amount], [Description], [TransactionDate], [Status], [OrderId], [CreateDate], [UpdateDate], [IsDeleted], [TrasactionCode]) VALUES (5, 2, N'OUT', 331, N'Thanh toán cho đơn hàng 20241019003818', CAST(N'2024-10-19T00:38:43.1385492' AS DateTime2), N'FAILED', 2, CAST(N'2024-10-19T00:38:33.4064551' AS DateTime2), CAST(N'2024-10-19T00:38:43.1385776' AS DateTime2), 0, 0)
SET IDENTITY_INSERT [dbo].[Transaction] OFF
GO
SET IDENTITY_INSERT [dbo].[Trip] ON 

INSERT [dbo].[Trip] ([Id], [Name], [RouteId], [OpenTicketDate], [EstimatedStartDate], [EstimatedEndDate], [ActualStartDate], [ActualEndDate], [Status], [IsTemplate], [DriverId], [UnsignName], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, N'template trip', 1, CAST(N'2024-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-06-03T00:00:00.0000000' AS DateTime2), CAST(N'2024-06-05T00:00:00.0000000' AS DateTime2), NULL, NULL, N'OPENING', 1, 1, N'template trip', CAST(N'2024-10-19T00:07:46.0450811' AS DateTime2), NULL, 0)
INSERT [dbo].[Trip] ([Id], [Name], [RouteId], [OpenTicketDate], [EstimatedStartDate], [EstimatedEndDate], [ActualStartDate], [ActualEndDate], [Status], [IsTemplate], [DriverId], [UnsignName], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, N'Chuyến xe 1', 1, CAST(N'2024-10-20T00:00:00.0000000' AS DateTime2), CAST(N'2024-10-21T08:00:00.0000000' AS DateTime2), CAST(N'2024-10-21T11:00:00.0000000' AS DateTime2), NULL, NULL, N'OPENING', 0, 4, N'Chuyen xe 1', CAST(N'2024-10-19T00:15:58.8778518' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[Trip] OFF
GO
SET IDENTITY_INSERT [dbo].[TripService] ON 

INSERT [dbo].[TripService] ([Id], [TripId], [ServiceId], [ServicePrice], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, 2, 1, 8, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0)
INSERT [dbo].[TripService] ([Id], [TripId], [ServiceId], [ServicePrice], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, 2, 2, 15, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0)
INSERT [dbo].[TripService] ([Id], [TripId], [ServiceId], [ServicePrice], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (3, 2, 3, 30, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[TripService] OFF
GO
SET IDENTITY_INSERT [dbo].[TripTicketType] ON 

INSERT [dbo].[TripTicketType] ([Id], [TripId], [TicketTypeId], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (1, 1, 1, CAST(N'2024-10-19T00:07:46.6066667' AS DateTime2), NULL, 0)
INSERT [dbo].[TripTicketType] ([Id], [TripId], [TicketTypeId], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, 2, 1, CAST(N'2024-10-19T00:15:58.9833333' AS DateTime2), NULL, 0)
INSERT [dbo].[TripTicketType] ([Id], [TripId], [TicketTypeId], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (3, 2, 2, CAST(N'2024-10-19T00:15:58.9833333' AS DateTime2), NULL, 0)
SET IDENTITY_INSERT [dbo].[TripTicketType] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [Email], [ConfirmEmail], [PasswordHash], [FullName], [DOB], [PhoneNumber], [Address], [Gender], [Status], [AvatarUrl], [GoogleId], [FCMToken], [Role], [UnsignFullName], [PIN], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, N'admin@example.com', 1, N'$2a$11$h81rA6tuBcB8IkOUOMPJFevGWZpcc7MLSU25QvovwSOthcGVe8iNK', N'Trần Hồng Thy', CAST(N'2002-07-30T00:00:00.0000000' AS DateTime2), N'0909113114', N'Long An', NULL, N'ACTIVE', N'https://cdn.discordapp.com/attachments/780808658262949898/1296849273530351638/IMG_8744.JPG?ex=6713c881&is=67127701&hm=817d9f23cf9e6d059a1cac57f9b718605569541fc49d79c5f39e0da0b436b7db&', NULL, NULL, N'ADMIN', N'Tran Hong Thy', NULL, CAST(N'2024-10-17T23:40:45.5588944' AS DateTime2), NULL, 0)
INSERT [dbo].[User] ([Id], [Email], [ConfirmEmail], [PasswordHash], [FullName], [DOB], [PhoneNumber], [Address], [Gender], [Status], [AvatarUrl], [GoogleId], [FCMToken], [Role], [UnsignFullName], [PIN], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (4, N'giaducdang@gmail.com', 1, N'$2a$11$lDLls3Amn3bsz.PXhleeyulvBIKc4.wfcmO1ZBHm8kMLMi5lkgfSK', N'Trần Tina', CAST(N'2003-07-30T00:00:00.0000000' AS DateTime2), N'0909113114', N'Long An', NULL, N'ACTIVE', NULL, NULL, NULL, N'USER', N'Tran Tina', NULL, CAST(N'2024-10-17T23:50:41.4893904' AS DateTime2), CAST(N'2024-10-18T19:35:24.2860477' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[Wallet] ON 

INSERT [dbo].[Wallet] ([Id], [UserId], [AccountBalance], [Status], [CreateDate], [UpdateDate], [IsDeleted]) VALUES (2, 4, 169, N'ACTIVE', CAST(N'2024-10-17T23:50:41.7200000' AS DateTime2), CAST(N'2024-10-19T00:36:00.8203470' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Wallet] OFF
GO
ALTER TABLE [dbo].[City] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[OrderDetail] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Otp] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Route] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[RouteStation] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Service] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ServiceTicket] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Station] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Ticket] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[TicketType] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Transaction] ADD  DEFAULT (N'') FOR [TransactionType]
GO
ALTER TABLE [dbo].[Transaction] ADD  DEFAULT (N'') FOR [Status]
GO
ALTER TABLE [dbo].[Transaction] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Transaction] ADD  DEFAULT ((0)) FOR [TrasactionCode]
GO
ALTER TABLE [dbo].[Trip] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[TripTicketType] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT (CONVERT([bit],(0))) FOR [ConfirmEmail]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Wallet] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK__Notificat__UserI__48CFD27E] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK__Notificat__UserI__48CFD27E]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_User]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK__OrderDeta__Order__0C85DE4D] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK__OrderDeta__Order__0C85DE4D]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK__OrderDeta__Ticke__0B91BA14] FOREIGN KEY([TicketId])
REFERENCES [dbo].[Ticket] ([Id])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK__OrderDeta__Ticke__0B91BA14]
GO
ALTER TABLE [dbo].[Route]  WITH CHECK ADD  CONSTRAINT [FK__Route__EndPoint__4E88ABD4] FOREIGN KEY([EndPoint])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[Route] CHECK CONSTRAINT [FK__Route__EndPoint__4E88ABD4]
GO
ALTER TABLE [dbo].[Route]  WITH CHECK ADD  CONSTRAINT [FK__Route__StartPoin__4D94879B] FOREIGN KEY([StartPoint])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[Route] CHECK CONSTRAINT [FK__Route__StartPoin__4D94879B]
GO
ALTER TABLE [dbo].[RouteStation]  WITH CHECK ADD  CONSTRAINT [FK__RouteStat__Route__59063A47] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Route] ([Id])
GO
ALTER TABLE [dbo].[RouteStation] CHECK CONSTRAINT [FK__RouteStat__Route__59063A47]
GO
ALTER TABLE [dbo].[RouteStation]  WITH CHECK ADD  CONSTRAINT [FK__RouteStat__Stati__59FA5E80] FOREIGN KEY([StationId])
REFERENCES [dbo].[Station] ([Id])
GO
ALTER TABLE [dbo].[RouteStation] CHECK CONSTRAINT [FK__RouteStat__Stati__59FA5E80]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK__Service__RouteId__73BA3083] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Route] ([Id])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK__Service__RouteId__73BA3083]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK__Service__Station__74AE54BC] FOREIGN KEY([StationId])
REFERENCES [dbo].[Station] ([Id])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK__Service__Station__74AE54BC]
GO
ALTER TABLE [dbo].[ServiceTicket]  WITH CHECK ADD  CONSTRAINT [FK__ServiceTi__Servi__7D439ABD] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([Id])
GO
ALTER TABLE [dbo].[ServiceTicket] CHECK CONSTRAINT [FK__ServiceTi__Servi__7D439ABD]
GO
ALTER TABLE [dbo].[ServiceTicket]  WITH CHECK ADD  CONSTRAINT [FK__ServiceTi__Ticke__7E37BEF6] FOREIGN KEY([TicketId])
REFERENCES [dbo].[Ticket] ([Id])
GO
ALTER TABLE [dbo].[ServiceTicket] CHECK CONSTRAINT [FK__ServiceTi__Ticke__7E37BEF6]
GO
ALTER TABLE [dbo].[Station]  WITH CHECK ADD  CONSTRAINT [FK_Station_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([Id])
GO
ALTER TABLE [dbo].[Station] CHECK CONSTRAINT [FK_Station_City]
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK__Ticket__TicketTy__6EF57B66] FOREIGN KEY([TicketTypeId])
REFERENCES [dbo].[TicketType] ([Id])
GO
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [FK__Ticket__TicketTy__6EF57B66]
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK__Ticket__TripId__6E01572D] FOREIGN KEY([TripId])
REFERENCES [dbo].[Trip] ([Id])
GO
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [FK__Ticket__TripId__6E01572D]
GO
ALTER TABLE [dbo].[TicketType]  WITH CHECK ADD  CONSTRAINT [FK__TicketTyp__Route__6383C8BA] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Route] ([Id])
GO
ALTER TABLE [dbo].[TicketType] CHECK CONSTRAINT [FK__TicketTyp__Route__6383C8BA]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK__Transacti__Walle__160F4887] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK__Transacti__Walle__160F4887]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK__Transaction__Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK__Transaction__Order]
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD  CONSTRAINT [FK__Trip__RouteId__5EBF139D] FOREIGN KEY([RouteId])
REFERENCES [dbo].[Route] ([Id])
GO
ALTER TABLE [dbo].[Trip] CHECK CONSTRAINT [FK__Trip__RouteId__5EBF139D]
GO
ALTER TABLE [dbo].[TripService]  WITH CHECK ADD  CONSTRAINT [FK__TripServi__Servi__778AC167] FOREIGN KEY([TripId])
REFERENCES [dbo].[Trip] ([Id])
GO
ALTER TABLE [dbo].[TripService] CHECK CONSTRAINT [FK__TripServi__Servi__778AC167]
GO
ALTER TABLE [dbo].[TripService]  WITH CHECK ADD  CONSTRAINT [FK__TripServi__Servi__787EE5A0] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([Id])
GO
ALTER TABLE [dbo].[TripService] CHECK CONSTRAINT [FK__TripServi__Servi__787EE5A0]
GO
ALTER TABLE [dbo].[TripTicketType]  WITH CHECK ADD  CONSTRAINT [FK__TripTicke__Ticke__693CA210] FOREIGN KEY([TicketTypeId])
REFERENCES [dbo].[TicketType] ([Id])
GO
ALTER TABLE [dbo].[TripTicketType] CHECK CONSTRAINT [FK__TripTicke__Ticke__693CA210]
GO
ALTER TABLE [dbo].[TripTicketType]  WITH CHECK ADD  CONSTRAINT [FK__TripTicke__TripI__68487DD7] FOREIGN KEY([TripId])
REFERENCES [dbo].[Trip] ([Id])
GO
ALTER TABLE [dbo].[TripTicketType] CHECK CONSTRAINT [FK__TripTicke__TripI__68487DD7]
GO
ALTER TABLE [dbo].[Wallet]  WITH CHECK ADD  CONSTRAINT [FK_Wallet_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Wallet] CHECK CONSTRAINT [FK_Wallet_User]
GO
