const app = require('./app');
const PORT = process.env.PORT || 8080;
app.listen(PORT, () => console.log(`Node service listening on :${PORT}`));
