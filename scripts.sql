create table users (
	uuid UUID primary key,
	user_name varchar(50) unique not null,
	email text unique,
	first_name varchar(100) not null,
	last_name varchar(100) not null,
	date_created timestamptz not null,
	date_modified timestamptz,
	password text not null
)