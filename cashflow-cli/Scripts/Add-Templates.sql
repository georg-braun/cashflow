CREATE TABLE "Templates" (
    "Id" uuid NOT NULL,
    "Amount" double precision NOT NULL,
    "Interval" interval NOT NULL,
    "Note" text NOT NULL,
    "CategoryId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "PK_Templates" PRIMARY KEY ("Id")
);
