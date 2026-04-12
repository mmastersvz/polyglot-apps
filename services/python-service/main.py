from fastapi import FastAPI

app = FastAPI(title="Python Service")


@app.get("/")
def hello():
    return {"message": "Hello from Python service!"}


@app.get("/health")
def health():
    return {"status": "ok"}
