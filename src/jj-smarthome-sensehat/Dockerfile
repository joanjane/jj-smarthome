FROM node:8-buster

# Move into a working directory
WORKDIR /usr/src/app

RUN apt-get update && apt-get install build-essential \
    python \
    make \
    g++ \
    nano \
    -y

# Copy our package.json into the container. This tells npm about the
# module that we're trying to run
COPY package.json ./

# Let's install our dependencies
RUN npm install

# And now let's copy our code into the container
COPY . ./

# Execute our code when the continer starts up
CMD ["npm", "start"]
