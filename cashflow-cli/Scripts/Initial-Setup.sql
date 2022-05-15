CREATE TABLE "Categories"
(
    "Id"     uuid NOT NULL,
    "Name"   text NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
);

CREATE TABLE "MoneyMovements"
(
    "Id"         uuid                     NOT NULL,
    "Amount"     double precision         NOT NULL,
    "Timestamp"  timestamp with time zone NOT NULL,
    "Note"       text                     NOT NULL,
    "CategoryId" uuid                     NOT NULL,
    "UserId"     uuid                     NOT NULL,
    CONSTRAINT "PK_MoneyMovements" PRIMARY KEY ("Id")
);

CREATE TABLE "Users"
(
    "Id"             uuid                     NOT NULL,
    "AuthProviderId" text                     NOT NULL,
    "Created"        timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);


