insert into ActionKeys 
	([key],value) values ('mabc','')
go

insert into actionstepsqlitem
	(actionname,stepsql) values ('getproducts','select * from products')

insert into actionstepsqlitem
	(actionname,stepsql) values ('getproducts by id','select * from products where id=''{0}''')

insert into actionstepsqlitem
	(actionname,stepsql) values ('getproducts by name','select * from products where name=''{0}''')

go

insert into actiongrouplist
	(akid,assid) values (1,2)
insert into actiongrouplist
	(akid,assid) values (1,3)
insert into actiongrouplist
	(akid,assid) values (1,4)

go