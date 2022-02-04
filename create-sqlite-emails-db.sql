create table if not exists Emails (
    Id          integer     primary key autoincrement,
    Subject     text        not null,
    Body        text        not null
);

create table if not exists EmailsRecipients (
    EmailId     integer,
    Recipient   text        not null,

    foreign key (EmailId) references Emails(Id)
);

create table if not exists EmailsStatuses (
    EmailId         integer     primary key,
    SendedAt        text        not null,
    Result          text        not null,
    FailedMessage   text,

    foreign key (EmailId) references Emails(Id)
);
