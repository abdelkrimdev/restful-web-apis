db.createUser({
  user: "iodine",
  pwd: "secret",
  roles: [
    { role: "dbOwner", db: "supaTrupa" }
  ]
});