create database first_snow
  with owner = postgres
       encoding = 'UTF8'
       tablespace = pg_default
       lc_collate = 'Russian_Russia.1251'
       lc_ctype = 'Russian_Russia.1251'
       connection limit = -1;
	  
create schema main
  authorization postgres;	  
	  
create schema schedule
  authorization postgres;	  
	  
create schema "user"
  authorization postgres;	  
	  
create schema history
  authorization postgres;	  
	  
create or replace function history_trigger() returns trigger as $$
declare
  _row record;
  _columns varchar;
  _base_columns varchar;
  _base varchar;
  begin
  
     if(tg_op ilike("update") and new is not distinct from old) then 
        return null;
     end if;
     
     _base = 'new';
     
     if(tg_op ilike("delete")) then
       _base = 'old';
     end if;
     
     select 
         array_to_string(array_agg(column_name), ', '),
         array_to_string(array_agg(_base||'.'||column_name), ', ')
     from information_schema.columns
       where table_schema = tg_table_schema and table_name = tg_table_name 
       into _columns, _base_columns;

     execute 'insert into '||tg_table_schema||'.h_'||tg_table_name||
             '('||_columns||', op_type) select '||_base_columns||', '||tg_op;
     
     return null;
  end;
$$ language plpgsql;	  
	  
	  -- TODO
create or replace function create_history_trigger() returns event_trigger as $$
declare
  
  begin
  
     
     
  end;
$$ language plpgsql;	  
	  
	  
	  
	  
	  