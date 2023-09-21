CREATE TABLE "Websites" (
	"Id" text NOT NULL,
	"Url" text NOT NULL,
	"BoundaryRegExp" text NOT NULL,
	"CrawlFrequency" integer NOT NULL,
	"Label" text NOT NULL,
	"IsActive" bool NOT NULL,
	"Tags" text NOT NULL,
	"LastExecutionTime" timestamp NOT NULL,
	"CrawledData" text,
	CONSTRAINT Websites_pk PRIMARY KEY ("Id")
);


CREATE TABLE "Executions" (
	"Id" text,
	"WebsiteId" text NOT NULL,
	"StartTime" timestamp NOT NULL,
	"EndTime" timestamp NOT NULL,
	"Status" integer NOT NULL,
	"CrawledSitesCount" integer NOT NULL,
	"WebsiteLabel" text NOT NULL,
	CONSTRAINT Executions_pk PRIMARY KEY ("Id")
);
ALTER TABLE "Executions" ADD CONSTRAINT Websites_fk FOREIGN KEY ("WebsiteId")
REFERENCES "Websites" ("Id") MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;

CREATE TABLE "CrawledDatas" (
	"Id" text NOT NULL,
	"Url" text NOT NULL,
	"Title" text NOT NULL,
	"CrawlTime" interval NOT NULL,
	"StartTime" timestamp NOT NULL,
	"IsRestricted" bool NOT NULL,
	"Links" text NOT NULL,
	"ExecutionId" text NOT NULL,
	CONSTRAINT CrawledDatas_pk PRIMARY KEY ("Id")
);

ALTER TABLE "CrawledDatas" ADD CONSTRAINT Executions_fk FOREIGN KEY ("ExecutionId")
REFERENCES "Executions" ("Id") MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;