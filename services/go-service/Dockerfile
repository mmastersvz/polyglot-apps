FROM alpine:3.19

# Install certificates for secure outgoing calls (APIs, etc.)
RUN apk --no-cache add ca-certificates

WORKDIR /root/

COPY app .

# Ensure the binary has execution permissions
RUN chmod +x app

EXPOSE 8080

CMD ["./app"]