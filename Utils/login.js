// login.js npm install @supabase/supabase-js

import { createClient } from '@supabase/supabase-js';

const [,, email, password] = process.argv;

if (!email || !password) {
  console.error('Usage: node login.js <email> <password>');
  process.exit(1);
}

const supabaseUrl = 'https://mgdklegnrjrzbnomclfs.supabase.co'; // remplace par ton URL Supabase
const supabaseAnonKey = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im1nZGtsZWducmpyemJub21jbGZzIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDg1Njg0NzgsImV4cCI6MjA2NDE0NDQ3OH0.rI-PU8UlL3xaBMd8pIzpHjO45ZoLKf51NWNei3LrlKU';                // remplace par ta clé anon publique

const supabase = createClient(supabaseUrl, supabaseAnonKey);

const run = async () => {
  const { data, error } = await supabase.auth.signInWithPassword({
    email,
    password
  });

  if (error) {
    console.error('❌ Connexion échouée :', error.message);
  } else {
    console.log('✅ Connecté ! Voici ton access_token :\n');
    console.log(data.session.access_token);
  }
};

run();
