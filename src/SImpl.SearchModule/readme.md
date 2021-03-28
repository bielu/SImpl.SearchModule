# SImpl WebApi Template

**Docker**

Build an image from this project
```shell
docker build --no-cache -f Dockerfile -t simpl.searchmodule .
```

Start the container
```shell
docker run -d -p 8080:80 simpl.searchmodule:latest
```

After starting the container point your browser at localhost:8080
