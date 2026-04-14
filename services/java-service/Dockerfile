
FROM eclipse-temurin:21-jre-alpine

ENV PORT=8080
ENV CLASSPATH=/opt/lib
EXPOSE 8080

# copy pom.xml and wildcards to avoid this command failing if there's no target/lib directory
COPY pom.xml target/lib* /opt/lib/

COPY target/*.jar /opt/app.jar

COPY docker-entrypoint.sh /opt/docker-entrypoint.sh
RUN chmod +x /opt/docker-entrypoint.sh

WORKDIR /opt
ENTRYPOINT ["/opt/docker-entrypoint.sh"]