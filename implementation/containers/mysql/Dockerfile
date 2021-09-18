FROM mysql
ENV MYSQL_ROOT_PASSWORD admin
ENV MYSQL_DATABASE st_and_db_mysql_database
COPY ./init.sql /docker-entrypoint-initdb.d/
